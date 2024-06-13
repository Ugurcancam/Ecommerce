using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;
using Ecommerce.Core.Entity;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Ecommerce.Core.Services;
using System.Security.Claims;

namespace Ecommerce.Web.Controllers;

public class HomeController : Controller
{

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IFavoriteService _favoriteService;
    private readonly IBasketService _basketService;
    private readonly ICategoryService _categoryService;
    private readonly IOrderService _orderService;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IProductService productService, IFavoriteService favoriteService, IBasketService basketService, ICategoryService categoryService, IOrderService orderService)
    {

        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _productService = productService;
        _favoriteService = favoriteService;
        _basketService = basketService;
        _categoryService = categoryService;
        _orderService = orderService;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model)
    {
        //Login with email and password
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel model)
    {
        var user = new AppUser()
        {
            Id = Guid.NewGuid().ToString(),
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            UserName = model.Email
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Home");
        }

        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Description);
        }

        return View();
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        System.Console.WriteLine("User logged out");
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAllAsync());
    }
    public async Task<IActionResult> ProductDetail(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        var similarProducts = await _productService.GetSimilarProducts(product.CategoryId);
        var model = new ProductViewModel
        {
            Product = product,
            SimilarProducts = similarProducts
        };
        return View(model);
    }
    public async Task<IActionResult> Favorites()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        var favoriteProducts = await _favoriteService.GetFavorites(userId);

        var model = favoriteProducts.Select(product => new FavoriteProductViewModel
        {
            ProductId = product.ProductId,
            Name = product.Product.Name,
            Description = product.Product.Description,
            Price = product.Product.Price
        }).ToList();

        return View(model);
    }
    public async Task<IActionResult> AddProductToFavorite(int productId, string returnUrl)
    {
        //Get the logged in users id
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        await _favoriteService.AddFavorite(userId, productId);
        return RedirectToAction(returnUrl);
    }
    public async Task<IActionResult> RemoveProductFromFavorite(int productId, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _favoriteService.RemoveFavorite(userId, productId);
        return RedirectToAction(returnUrl);
    }

    public async Task<IActionResult> Basket()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        var basket = await _basketService.GetBasketByUserIdAsync(userId);
        var model = basket.BasketItems.Select(ci => new BasketItemViewModel
        {
            ProductId = ci.ProductId,
            Name = ci.Product.Name,
            Price = ci.Product.Price,
            Quantity = ci.Quantity
        }).ToList();

        return View(model);
    }
    //Daha sonra quantity parametresi eklenecek
    public async Task<IActionResult> AddProductToBasket(int productId, string returnUrl, bool returnProductId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        await _basketService.AddToBasketAsync(userId, productId, 1);

        // returnProductId değeri true ise returnUrl'e productId ile geri dönüyoruz.(ProductDetail sayfası gibi id'ye ihtiyaç duyulan sayfalarda)
        if (returnProductId)
        {
            return RedirectToAction(returnUrl, new { id = productId });
        }
        else
        {
            return RedirectToAction(returnUrl);
        }
    }


    public async Task<IActionResult> ReduceQuantity(int productId, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        await _basketService.ReduceQuantityAsync(userId, productId);
        return RedirectToAction(returnUrl);
    }
    public async Task<IActionResult> RemoveProductFromBasket(int productId, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        await _basketService.RemoveFromBasketAsync(userId, productId);
        return RedirectToAction(returnUrl);
    }


    public IActionResult Dashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    public async Task<IActionResult> CategoryWithProducts(int id)
    {
        return View(await _categoryService.GetWithProductsByIdAsync(id));
    }

    public async Task<IActionResult> Cart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            //Sayfada kullanıcı girişi yapmamışsa boş sepet döndür, sepet count'larını gösterdiğimizden dolayı çökmemesi için boş bir model döndürmemiz lazım.
            return View(new List<BasketItemViewModel>());
        }
        var basket = await _basketService.GetBasketByUserIdAsync(userId);
        var model = basket.BasketItems.Select(p => new BasketItemViewModel
        {
            ProductId = p.ProductId,
            Name = p.Product.Name,
            Price = p.Product.Price,
            Quantity = p.Quantity
        }).ToList();

        return View(model);
    }
    public IActionResult CreateOrder()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderViewModel orderViewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login");
        }
        try
        {
            var order = new Order
            {
                City = orderViewModel.City,
                District = orderViewModel.District,
                PostalCode = orderViewModel.PostalCode,
                OrderNote = orderViewModel.OrderNote,
                DeliveryAddress = orderViewModel.DeliveryAddress,
                BillingAddress = orderViewModel.BillingAddress
            };
           var newOrder = await _orderService.CreateOrderAsync(userId, order.City, order.District, order.PostalCode, order.OrderNote, order.DeliveryAddress, order.BillingAddress);
            return RedirectToAction("OrderConfirmation", new { orderId = newOrder.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _orderService.GetUserOrdersAsync(userId);
        var order = orders.FirstOrDefault(o => o.Id == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> UserOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return View(orders);
    }
    public async Task<IActionResult> UserOrderDetails(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await _orderService.GetOrderDetailsByUserAsync(userId, orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }


}

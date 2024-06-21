using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;
using Ecommerce.Core.Entity;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Ecommerce.Core.Services;
using System.Security.Claims;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;
using Ecommerce.Service.Services;
using Ecommerce.Core.Dtos;
using System.Globalization;
using Newtonsoft.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

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
    private readonly PaymentService _paymentService;
    private readonly IyzipayOptions _iyzipayOptions;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IProductService productService, IFavoriteService favoriteService, IBasketService basketService, ICategoryService categoryService, IOrderService orderService, PaymentService paymentService, IOptions<IyzipayOptions> iyzipayOptions)
    {

        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _productService = productService;
        _favoriteService = favoriteService;
        _basketService = basketService;
        _categoryService = categoryService;
        _orderService = orderService;
        _paymentService = paymentService;
        _iyzipayOptions = iyzipayOptions.Value;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(UserDto model)
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
    public async Task<IActionResult> Register(UserDto model)
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
        var model = new ProductDto
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

        var model = favoriteProducts.Select(product => new FavoriteProductDto
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
        var model = basket.BasketItems.Select(ci => new BasketItemDto
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
            return View(new List<BasketItemDto>());
        }
        var basket = await _basketService.GetBasketByUserIdAsync(userId);
        var model = basket.BasketItems.Select(p => new BasketItemDto
        {
            ProductId = p.ProductId,
            Name = p.Product.Name,
            Price = p.Product.Price,
            Quantity = p.Quantity
        }).ToList();
        var orderGuid = "asdasd";
        HttpContext.Session.SetString("orderGuid", orderGuid);
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
    // [HttpPost]
    // public async Task<IActionResult> CreateOrder(OrderDto orderDto)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     if (userId == null)
    //     {
    //         return RedirectToAction("Login");
    //     }
    //     try
    //     {
    //         var newOrder = await _orderService.CreateOrderAsync(userId, orderDto);
    //         return RedirectToAction("OrderConfirmation", new { orderId = newOrder.Id });
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         ModelState.AddModelError("", ex.Message);
    //         return RedirectToAction("Index", "Home");
    //     }
    // }
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
    public ActionResult Deneme()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Pay(OrderDto orderDto)
    {
        Payment payment = new Payment();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var basket = await _basketService.GetBasketByUserIdAsync(userId);
        var conversationId = Guid.NewGuid().ToString();

        // Initialize Iyzipay
        Iyzipay.Options options = new Iyzipay.Options
        {
            ApiKey = _iyzipayOptions.ApiKey,
            SecretKey = _iyzipayOptions.SecretKey,
            BaseUrl = _iyzipayOptions.BaseUrl
        };

        CreatePaymentRequest request = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = conversationId,
            Price = basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price).ToString("F2", CultureInfo.InvariantCulture),
            PaidPrice = basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price).ToString("F2", CultureInfo.InvariantCulture),
            Currency = Currency.TRY.ToString(),
            Installment = 1,
            BasketId = "B67832",
            PaymentChannel = PaymentChannel.WEB.ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),
            CallbackUrl = Url.Action("Callback", "Home", orderDto, Request.Scheme)
        };

        PaymentCard paymentCard = new PaymentCard
        {
            CardHolderName = orderDto.CardHolderName,
            CardNumber = orderDto.CardNumber,
            ExpireMonth = orderDto.ExpireMonth,
            ExpireYear = orderDto.ExpireYear,
            Cvc = orderDto.CVV,
            RegisterCard = 0
        };
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer
        {
            Id = "BY789",
            Name = orderDto.Name,
            Surname = orderDto.Surname,
            GsmNumber = orderDto.PhoneNumber,
            Email = "email@hotmail.com",
            IdentityNumber = "1235673",
            LastLoginDate = "2015-10-05 12:43:35",
            RegistrationDate = "2013-04-21 15:12:09",
            RegistrationAddress = orderDto.DeliveryAddress,
            Ip = "85.34.78.112",
            City = orderDto.City,
            Country = "Turkey",
            ZipCode = orderDto.PostalCode,
        };
        request.Buyer = buyer;

        Address shippingAddress = new Address
        {
            ContactName = orderDto.Name + " " + orderDto.Surname,
            City = orderDto.City,
            Country = "Turkey",
            Description = orderDto.DeliveryAddress,
            ZipCode = orderDto.PostalCode
        };
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address
        {
            ContactName = orderDto.Name + " " + orderDto.Surname,
            City = orderDto.City,
            Country = "Turkey",
            Description = orderDto.BillingAddress,
            ZipCode = orderDto.PostalCode
        };
        request.BillingAddress = billingAddress;

        List<Iyzipay.Model.BasketItem> basketItems = new List<Iyzipay.Model.BasketItem>();
        foreach (var basketItem in basket.BasketItems)
        {
            for (int i = 0; i < basketItem.Quantity; i++)
            {
                Iyzipay.Model.BasketItem firstBasketItem = new Iyzipay.Model.BasketItem();
                firstBasketItem.Id = "BI" + basketItem.Id;
                firstBasketItem.Name = basketItem.Product.Name;
                firstBasketItem.Category1 = "Kategori";
                firstBasketItem.Category2 = "Kategori 2";
                firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                firstBasketItem.Price = (basketItem.Product.Price).ToString("F2", CultureInfo.InvariantCulture);
                basketItems.Add(firstBasketItem);

            }
        }
        request.BasketItems = basketItems;
        // Send the payment request
        ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);

        _paymentService.PrintResponse<ThreedsInitialize>(threedsInitialize);



        if (threedsInitialize.Status == "success")
        {
            await _orderService.CreateOrderAsync(userId, conversationId, orderDto);
            return Content(threedsInitialize.HtmlContent, "text/html");
        }
        else
        {
            Console.WriteLine("Hata mesajı" + payment.ErrorCode + payment.ErrorMessage);
            // Payment failed
            return RedirectToAction("Failure");
        }
    }
    [HttpPost]
    public async Task<ActionResult> Callback()
    {
        try
        {
            string paymentId = Request.Form["paymentId"];
            string conversationId = Request.Form["conversationId"];
            System.Console.WriteLine("ConvIdCallback: " + conversationId);

            var order = await _orderService.GetByPaymentIdAsync(conversationId);
            if (order == null)
            {
                System.Console.WriteLine("order is null");
            }
            System.Console.WriteLine(order.PaymentId);

            if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(conversationId))
            {
                Console.WriteLine("Payment token or conversation ID is missing");
                return RedirectToAction("Failure");
            }

            Iyzipay.Options options = new Iyzipay.Options
            {
                ApiKey = _iyzipayOptions.ApiKey,
                SecretKey = _iyzipayOptions.SecretKey,
                BaseUrl = _iyzipayOptions.BaseUrl
            };
            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                PaymentId = paymentId
            };

            ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

            if (threedsPayment.Status == "success")
            {

                order.OrderState = OrderState.CargoPending;
                order.IsPaid = true;
                await _orderService.UpdateAsync(order);
                return RedirectToAction("Success");
            }
            else
            {
                // Payment failed
                Console.WriteLine("Error: " + threedsPayment.ErrorCode + " - " + threedsPayment.ErrorMessage);
                return RedirectToAction("Failure");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction("Failure");
        }
    }
    public ActionResult Success()
    {
        return View();
    }

    public ActionResult Failure()
    {
        return View();
    }
}


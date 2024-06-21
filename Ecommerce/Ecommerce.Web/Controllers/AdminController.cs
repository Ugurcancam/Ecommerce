using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Dtos.Product;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Services;
using Ecommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IService<ProductFeature> _productFeatureService;
        private readonly IAltCategoryService _altCategoryService;
        private readonly IOrderService _orderService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private IMapper _mapper;

        public AdminController(IProductService productService, ICategoryService categoryService, IMapper mapper, IService<ProductFeature> productFeatureService, IAltCategoryService altCategoryService, IOrderService orderService, IBlogService blogService, IBlogCategoryService blogCategoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _altCategoryService = altCategoryService;
            _productFeatureService = productFeatureService;
            _orderService = orderService;
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Products()
        {
            return View(await _productService.GetProductsWithCategory());
        }
        public async Task<IActionResult> AddProduct()
        {
            var model = new ProductDto();
            model.Categories = await _categoryService.GetAllAsync();
            model.AltCategories = await _altCategoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto model)
        {
            await _productService.AddAsync(_mapper.Map<Product>(model));
            return RedirectToAction("Products");
        }
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var model = _mapper.Map<ProductDto>(await _productService.GetByIdAsync(id));
            model.Categories = await _categoryService.GetAllAsync();
            model.AltCategories = await _altCategoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductDto model)
        {
            await _productService.UpdateAsync(_mapper.Map<Product>(model));
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> RemoveProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Categories()
        {
            return View(await _categoryService.GetAllAsync());
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto model)
        {
            await _categoryService.AddAsync(_mapper.Map<Category>(model));
            return RedirectToAction("Categories");
        }
        public async Task<IActionResult> UpdateCategory(int id)
        {
            return View(_mapper.Map<CategoryDto>(await _categoryService.GetByIdAsync(id)));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDto model)
        {
            await _categoryService.UpdateAsync(_mapper.Map<Category>(model));
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> RemoveCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            await _categoryService.RemoveAsync(category);
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> AltCategories()
        {
            return View(await _altCategoryService.GetAllWithCategoryAsync());
        }
        public async Task<IActionResult> AddAltCategoryAsync()
        {
            var model = new AltCategoryDto();
            model.Categories = await _categoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAltCategory(AltCategoryDto model)
        {
            await _altCategoryService.AddAsync(_mapper.Map<AltCategory>(model));
            return RedirectToAction("AltCategories");
        }
        public async Task<IActionResult> UpdateAltCategory(int id)
        {
            var model = _mapper.Map<AltCategoryDto>(await _altCategoryService.GetByIdAsync(id));
            model.Categories = await _categoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAltCategory(AltCategoryDto model)
        {
            await _altCategoryService.UpdateAsync(_mapper.Map<AltCategory>(model));
            return RedirectToAction("AltCategories");
        }
        public async Task<IActionResult> RemoveAltCategory(int id)
        {
            var altCategory = await _altCategoryService.GetByIdAsync(id);
            await _altCategoryService.RemoveAsync(altCategory);
            return RedirectToAction("AltCategories");
        }

        public async Task<IActionResult> CargoPendingOrders()
        {
            return View(await _orderService.GetCargoPendingOrdersWithUser());
        }
        public async Task<IActionResult> ShippedOrders()
        {
            return View(await _orderService.GetShippedOrdersWithUser());
        }
        public async Task<IActionResult> DeliveredOrders()
        {
            return View(await _orderService.GetDeliveredOrdersWithUser());
        }
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            return View(_mapper.Map<OrderDto>(await _orderService.GetOrderDetailsAsync(orderId)));
        }

        public async Task<IActionResult> Blogs()
        {
            var blogs = await _blogService.GetAllWithCategoryAsync();
            return View(blogs);
        }

        public async Task<IActionResult> AddBlog()
        {
            var model = new BlogDto();
            model.BlogCategories = await _blogCategoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddBlog(BlogDto model)
        {
            await _blogService.AddAsync(_mapper.Map<Blog>(model));
            return RedirectToAction("Blogs");
        }
        public async Task<IActionResult> UpdateBlog(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            var model = _mapper.Map<BlogDto>(blog);
            model.BlogCategories = await _blogCategoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBlog(BlogDto model)
        {
            await _blogService.UpdateAsync(_mapper.Map<Blog>(model));
            return RedirectToAction("Blogs");
        }
        public async Task<IActionResult> RemoveBlog(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            await _blogService.RemoveAsync(blog);
            return RedirectToAction("Blogs");
        }
        public async Task<IActionResult> BlogCategories()
        {
            var blogCategories = await _blogCategoryService.GetAllAsync();
            return View(blogCategories);
        }
        public IActionResult AddBlogCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBlogCategory(BlogCategoryDto model)
        {
            await _blogCategoryService.AddAsync(_mapper.Map<BlogCategory>(model));
            return RedirectToAction("BlogCategories");
        }
        public async Task<IActionResult> UpdateBlogCategory(int id)
        {
            return View(_mapper.Map<BlogCategoryDto>(await _blogCategoryService.GetByIdAsync(id)));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBlogCategory(BlogCategoryDto model)
        {
            await _blogCategoryService.UpdateAsync(_mapper.Map<BlogCategory>(model));
            return RedirectToAction("BlogCategories");
        }
        public async Task<IActionResult> RemoveBlogCategory(int id)
        {
            var blogCategory = await _blogCategoryService.GetByIdAsync(id);
            await _blogCategoryService.RemoveAsync(blogCategory);
            return RedirectToAction("BlogCategories");
        }
    }
}
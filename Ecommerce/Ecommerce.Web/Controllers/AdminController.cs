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
        private IMapper _mapper;

        public AdminController(IProductService productService, ICategoryService categoryService, IMapper mapper, IService<ProductFeature> productFeatureService, IAltCategoryService altCategoryService, IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _altCategoryService = altCategoryService;
            _productFeatureService = productFeatureService;
            _orderService = orderService;
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

        public async Task<IActionResult> Orders()
        {
            return View( await _orderService.GetAllWithUsers());
        }
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            return View(_mapper.Map<OrderDto>(await _orderService.GetOrderDetailsAsync(orderId)));
        }
    }
}
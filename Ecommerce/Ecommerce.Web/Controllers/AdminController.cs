using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;

        public AdminController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
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
            var model = new ProductViewModel();
            model.Categories = await _categoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
                await _productService.AddAsync(_mapper.Map<Product>(model));
                return RedirectToAction("Products");
        }
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var model = _mapper.Map<ProductViewModel>(await _productService.GetByIdAsync(id));
            model.Categories = await _categoryService.GetAllAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewModel model)
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
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            await _categoryService.AddAsync(_mapper.Map<Category>(model));
            return RedirectToAction("Categories");
        }
        public async Task<IActionResult> UpdateCategory(int id)
        {
            return View(_mapper.Map<CategoryViewModel>(await _categoryService.GetByIdAsync(id)));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel model)
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
    }
}
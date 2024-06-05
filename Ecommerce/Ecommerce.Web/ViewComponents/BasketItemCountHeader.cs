using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Core.Services;
using Ecommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.ViewComponents
{
    public class BasketItemCountHeaderViewComponent : ViewComponent
    {
        private readonly IBasketService _basketService;

        public BasketItemCountHeaderViewComponent(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                //return to login
                return View();
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

    }
}
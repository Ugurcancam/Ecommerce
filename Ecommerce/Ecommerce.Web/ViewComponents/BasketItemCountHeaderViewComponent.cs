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
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                //Sayfada kullanıcı girişi yapmamışsa boş sepet döndür, sepet count'larını gösterdiğimizden dolayı çökmemesi için boş bir model döndürmemiz lazım.
                return View(new List<BasketItemViewModel>());
            }
            var basket = await _basketService.GetBasketByUserIdAsync(userId);
            var model = basket?.BasketItems?.Select(p => new BasketItemViewModel
            {
                ProductId = p.ProductId,
                Name = p.Product.Name,
                Price = p.Product.Price,
                Quantity = p.Quantity
            }).ToList() ?? new List<BasketItemViewModel>();

            return View(model);
        }

    }
}
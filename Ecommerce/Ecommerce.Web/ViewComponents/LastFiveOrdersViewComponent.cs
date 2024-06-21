using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.ViewComponents
{
    public class LastFiveOrdersViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        public LastFiveOrdersViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _orderService.GetLastFiveOrdersWithUser());
        }
    }
}
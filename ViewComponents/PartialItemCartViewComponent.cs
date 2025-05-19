using Eshopp.Models; // Điều chỉnh namespace theo dự án
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Eshopp.Components
{
    public class PartialItemCartViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PartialItemCartViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

            return View(cart);
        }
    }
}
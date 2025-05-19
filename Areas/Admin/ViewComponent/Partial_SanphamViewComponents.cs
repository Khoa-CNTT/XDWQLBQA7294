using X.PagedList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShopp.Models.EF;
using EShopp.Repository;
using X.PagedList.Extensions;

namespace Eshopp.Areas.Admin.ViewComponents
{
    [ViewComponent(Name = "Partial_Sanpham")]
    public class Partial_SanphamViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Partial_SanphamViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int orderId, int page = 1, int pageSize = 15)
        {
            // Fetch OrderDetails for the specific Order, including the Product navigation property
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product) // Ensure the Product navigation property is loaded
                .ToListAsync();

            // Apply pagination
            var items = orderDetails.ToPagedList(page, pageSize);

            return View(items);
        }
    }
}
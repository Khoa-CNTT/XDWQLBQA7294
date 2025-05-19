using System.ComponentModel;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopp.ViewComponents
{
    public class Partial_ProductSales : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Partial_ProductSales(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Products
                .Where(x => x.IsSale && x.IsActive)
                .Include(x => x.ProductImages)  // Quan trọng: load cả ảnh
                .Take(12)
                .ToListAsync();  // Sử dụng ToListAsync thay vì Task.Run

            return View(items);
        }
    }
}

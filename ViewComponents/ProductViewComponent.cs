using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EShopp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProductViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? cateid = null)
        {
            var query = _context.Products
                .Include(p => p.ProductImages)  // Lấy danh sách ảnh sản phẩm
                .AsQueryable(); // Đảm bảo kiểu dữ liệu là IQueryable trước khi lọc

            if (cateid.HasValue)
            {
                query = query.Where(p => p.ProductCategoryId == cateid.Value);
            }

            var items = await query
                .OrderByDescending(p => p.CreatedDate)  // Sắp xếp theo ngày tạo sau khi lọc
                .ToListAsync(); // Lấy dữ liệu từ database

            return View(items);  // Trả về danh sách sản phẩm
        }
       
    }
}

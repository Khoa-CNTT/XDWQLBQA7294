using Eshopp.Models.EF;
using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10; // Số bài viết trên mỗi trang
            IEnumerable<Product> items = _context.Products.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext)).ToList();
            }
            var pageIndex = page ?? 1; // Nếu page = null, mặc định là trang 1
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex; // Đảm bảo Page luôn là số hợp lệ

            return View(items);
        }
        public ActionResult Add(int id)
        {
            return View();
        }
    }
}

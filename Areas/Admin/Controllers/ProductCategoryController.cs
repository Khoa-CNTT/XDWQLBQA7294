using Eshopp.Models.EF;
using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;
namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]


    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public ProductCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10; // Số bài viết trên mỗi trang
            IEnumerable<ProductCategory> items = _context.ProductCategories.OrderByDescending(x => x.Id);
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
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);
                _context.ProductCategories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }
    }
}

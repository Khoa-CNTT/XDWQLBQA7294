using Microsoft.AspNetCore.Mvc;
using Eshopp.Models.EF;
using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        // Inject DbContext vào Controller
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            var model = new Product();
            ViewBag.ProductCategory = new SelectList(_context.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile ImageFile, int productId)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", uniqueFileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.Image = uniqueFileName;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, imagePath = "/images/products/" + uniqueFileName });
            }

            return Json(new { success = false });
        }

    }
}

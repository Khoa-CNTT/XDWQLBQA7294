
using Microsoft.AspNetCore.Mvc;
using Eshopp.Models.EF;
using EShopp.Repository;
namespace EShopp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action hiển thị danh sách danh mục
        public ActionResult Index()
        {
            var items = _context.Categories;
            return View(items);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {

                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);
                _context.Categories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _context.Categories.Find(id);
            return View(item);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Attach(model);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);
                _context.Entry(model).Property(x => x.Title).IsModified = true;
                _context.Entry(model).Property(x => x.Description).IsModified = true;
                _context.Entry(model).Property(x => x.Alias).IsModified = true;
                _context.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _context.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _context.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _context.Entry(model).Property(x => x.Position).IsModified = true;
                _context.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _context.Entry(model).Property(x => x.Modifiedby).IsModified = true;


                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var item = _context.Categories.Find(id);
            if (item != null)
            {
                // var DeleteItem=_context.Categories.Attach(item);
                _context.Categories.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}

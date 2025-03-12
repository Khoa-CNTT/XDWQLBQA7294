using Eshopp.Models.EF;
using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EShopp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var item = _context.News.OrderByDescending(x => x.id).ToList();
            return View(item);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAsync(News model, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Định nghĩa thư mục lưu ảnh
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news");

                    // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    // Lưu file vào thư mục wwwroot/images/news
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào database
                    model.Image = "/images/news/" + fileName;
                }

                model.CreatedDate = DateTime.Now;
                model.CategoryId = 3;
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);
                _context.News.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = _context.News.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(News model, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news", fileName);

                    // Lưu file vào thư mục wwwroot/images/news
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào database
                    model.Image = "/images/news/" + fileName;
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);
                _context.News.Attach(model);
                _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.News.Find(id);
            if (item != null)
            {
                _context.News.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

    
      [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _context.News.Find(id);
            if (item != null)
            {
                item.IsActive =!item.IsActive;
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });

        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items =ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach(var item in items)
                    {
                        var obj = _context.News.Find(Convert.ToInt32(item));
                        _context.News.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                return Json(new {success =true});

            }
            return Json(new {success=false});
        }
    }
}

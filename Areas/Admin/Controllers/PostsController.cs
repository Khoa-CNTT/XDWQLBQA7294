using Eshopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Searchtext,int? page)
        {
            var pageSize = 10; // Số bài viết trên mỗi trang
            IEnumerable<Posts> items = _context.Posts.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
               items = items.Where(x=>x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext)).ToList();
            }
            var pageIndex = page ?? 1; // Nếu page = null, mặc định là trang 1
            items = items.ToPagedList(pageIndex,pageSize);
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
        public async Task<ActionResult> AddAsync(Posts model, IFormFile ImageFile)
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
                _context.Posts.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = _context.Posts.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Posts model, IFormFile ImageFile)
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
                _context.Posts.Attach(model);
                _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.Posts.Find(id);
            if (item != null)
            {
                _context.Posts.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }


        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _context.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
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
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _context.Posts.Find(Convert.ToInt32(item));
                        _context.Posts.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}


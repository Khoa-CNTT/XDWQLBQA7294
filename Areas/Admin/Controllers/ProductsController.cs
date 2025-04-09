using Microsoft.AspNetCore.Mvc;
using EShopp.Models.EF;
using EShopp.Repository;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
        }
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10; // Số sản phẩm trên mỗi trang

            // Đảm bảo items là IQueryable và load luôn ProductCategory
            var items = _context.Products
                .Include(p => p.ProductCategory) // Load quan hệ với ProductCategory
                .OrderByDescending(x => x.id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }

            var pageIndex = page ?? 1; // Mặc định trang 1 nếu page = null
            var pagedItems = items.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex; // Đảm bảo Page luôn là số hợp lệ

            return View(pagedItems);
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.ProductCategoryId = GetCategories();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product model, List<IFormFile> Images, List<int> rDefault)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Dữ liệu nhập vào không hợp lệ.";
                    ViewBag.ProductCategoryId = GetCategories();
                    return View(model);
                }

                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                if (string.IsNullOrEmpty(model.Alias))
                    model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);

                Console.WriteLine($"Total Uploaded Images: {Images?.Count}");

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Lưu sản phẩm trước để có ID
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Product ID after saving: {model.id}");

                if (model.id <= 0)
                {
                    throw new Exception("Failed to save product. Product ID is invalid.");
                }

                List<ProductImage> productImages = new List<ProductImage>();
                string? firstImage = null;

                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        var imageFile = Images[i];

                        if (imageFile.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            var filePath = Path.Combine(uploadDir, fileName);
                            var dbPath = "/images/products/" + fileName;

                            try
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await imageFile.CopyToAsync(stream);
                                }
                                Console.WriteLine($"Image saved successfully: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error saving file: {ex.Message}");
                            }

                            if (i == 0)
                                firstImage = dbPath;

                            bool isDefault = rDefault != null && rDefault.Contains(i + 1);

                            productImages.Add(new ProductImage
                            {
                                ProductId = model.id,
                                Image = dbPath,
                                IsDefault = isDefault
                            });

                            Console.WriteLine($"Image added to DB: {dbPath}");
                        }
                    }
                }

                // Cập nhật ảnh đầu tiên vào bảng Product
                if (!string.IsNullOrEmpty(firstImage))
                {
                    model.Image = firstImage;
                    _context.Products.Update(model); // Cập nhật lại sản phẩm
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Updated Product Image: {model.Image}");
                }

                if (productImages.Count > 0)
                {
                    Console.WriteLine("Saving images to database...");
                    _context.ProductImages.AddRange(productImages);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Images saved successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                Console.WriteLine($"Error: {ex.Message}");
            }

            ViewBag.ProductCategoryId = GetCategories();
            return View(model);
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _context.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });

        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = _context.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });

        }
    
        private List<SelectListItem> GetCategories()
        {
            return _context.ProductCategories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                })
                .ToList();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = _context.Products.Find(id);
            if (item == null) return NotFound();

            ViewBag.ProductCategoryId = GetCategories(); // Sửa lại tên ViewBag
            return View(item);
        }
        [HttpPost]
        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.id == model.id);
                if (existingProduct == null) return NotFound();

                // Nếu không có ảnh mới, giữ lại ảnh cũ
                if (string.IsNullOrEmpty(model.Image))
                {
                    model.Image = existingProduct.Image;
                }
                model.ModifiedDate = DateTime.Now;
                model.Alias = Eshopp.Models.Common.Filter.FilterChar(model.Title);

                _context.Products.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.Products.Find(id);
            if (item != null)
            {
                _context.Products.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }


    }
}

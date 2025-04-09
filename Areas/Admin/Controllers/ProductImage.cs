using EShopp.Models.EF;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using EShopp.Repository;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index(int productId)
        {
            ViewBag.ProductId = productId;
            var item = _context.ProductImages.Where(x => x.ProductId == productId).ToList();
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var image = _context.ProductImages.Find(id);
            if (image == null)
            {
                return Json(new { success = false, message = "Ảnh không tồn tại." });
            }

            // Xóa file ảnh vật lý
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + image.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Xóa khỏi database
            _context.ProductImages.Remove(image);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> Images, int productId)
        {
            if (Images == null || Images.Count == 0)
            {
                return Json(new { success = false, message = "Không có ảnh nào được chọn." });
            }

            var uploadedImages = new List<object>();

            foreach (var file in Images)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newImage = new ProductImage
                {
                    ProductId = productId,
                    Image = "/images/products/" + fileName, // Sửa lại đường dẫn đúng
                    IsDefault = false
                };

                _context.ProductImages.Add(newImage);
                await _context.SaveChangesAsync();

                uploadedImages.Add(new { id = newImage.id, imageUrl = newImage.Image });
            }

            return Json(new { success = true, images = uploadedImages });
        }
    }
}

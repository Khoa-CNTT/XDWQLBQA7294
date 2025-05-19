using System.Data.Entity;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace Eshopp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Controller
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            var items = _context.Orders.OrderByDescending(x => x.CreatedDate);
            int pageSize = 15;
            int pageNumber = page ?? 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult View(int id)
        {
            var item = _context.Orders.Find(id);
            return View(item);
        }
        public IActionResult UpdateTT(int id, int trangthai)
        {
            var item = _context.Orders.Find(id);
            if (item != null)
            {
                _context.Orders.Attach(item);
                item.TypePayment = trangthai;
                _context.Entry(item).Property(x => x.TypePayment).IsModified = true;
                _context.SaveChanges();
                return Json(new { message = "success", Success = true });


            }
            return Json(new { message = "Unsuccess", Success = false });    

        }
    }
}

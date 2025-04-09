using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EShopp.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuTopViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.OrderBy(x => x.Position).ToList();
            return View(categories); // View này sẽ nằm trong "Views/Shared/Components/MenuTop/Default.cshtml"
        }



    }
}


using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Eshopp.ViewComponents
{
    public class MenuLeftViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuLeftViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int ? id)
        {
            if(id != null)
            {
                ViewBag.CateId = id;
            }
            var items = await Task.Run(() => _context.ProductCategories.ToList());
            return View(items); // Views/Shared/Components/MenuProductCategory/Default.cshtml
        }
    }
}

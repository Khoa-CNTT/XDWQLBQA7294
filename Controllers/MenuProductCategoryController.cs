using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EShopp.ViewComponents
{
    public class MenuProductCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuProductCategoryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await Task.Run(() => _context.ProductCategories.ToList());
            return View(items); // Views/Shared/Components/MenuProductCategory/Default.cshtml
        }
    }
}

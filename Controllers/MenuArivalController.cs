using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EShopp.ViewComponents
{
    public class MenuArivalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuArivalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await Task.Run(() => _context.ProductCategories.ToList());
            return View(items);  // View nằm ở "Views/Shared/Components/MenuArival/Default.cshtml"
        }
    }
}

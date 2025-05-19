using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;

public class PartialItemByCateIdViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public PartialItemByCateIdViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = await Task.Run(() =>
            _context.Products
                .Where(x => x.IsHome && x.IsActive)
                .Take(12)
                .ToList()
        );

        return View(items); // Sẽ tìm Views/Shared/Components/PartialItemByCateId/Default.cshtml
    }
}

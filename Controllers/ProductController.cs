using EShopp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("san-pham")]
public class ClientProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClientProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /san-pham
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategory) // Load cả danh mục
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedDate)
            .AsNoTracking() // Tối ưu hiệu suất
            .ToListAsync();

        return View(products);
    }

    // GET: /san-pham/danh-muc/womens-1
    [HttpGet("danh-muc/{alias}-{id}")]
    public async Task<IActionResult> ProductCategory(string alias, int id)
    {
        // Kiểm tra danh mục tồn tại
        var categoryExists = await _context.ProductCategories
            .AnyAsync(c => c.Id == id && c.Alias == alias);
        if (!categoryExists) return NotFound();

        var products = await _context.Products
            .Include(p => p.ProductImages)
            .Where(p => p.ProductCategoryId == id && p.IsActive)
            .AsNoTracking()
            .ToListAsync();

        ViewBag.CategoryName = await _context.ProductCategories
            .Where(c => c.Id == id)
            .Select(c => c.Title)
            .FirstOrDefaultAsync();
        var cate = _context.ProductCategories.Find(id);
        if (cate != null)
        {
            ViewBag.CateName=cate.Title;
        }
        ViewBag.CateId = id;
        return View(products);
    }
    [HttpGet("chi-tiet/{alias}-p{id}")] // Thêm route đầy đủ

    public async Task<IActionResult> Detail(string alias, int id)
    {
        var product = await _context.Products
            .Include(p => p.ProductImages)  // Load ảnh sản phẩm
            .Include(p => p.ProductCategory)  // Load danh mục
            .AsNoTracking()  // Tối ưu hiệu suất
            .FirstOrDefaultAsync(p => p.id == id);

        if (product == null)
        {
            return NotFound();
        }

        // Redirect nếu alias không khớp (quan trọng cho SEO)
        if (!string.Equals(product.Alias, alias, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToActionPermanent(nameof(Detail),
                new { alias = product.Alias, id = product.id });
        }

        return View(product);
    }
}
using Eshopp.Models;
using Eshopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ReturnUrlParameter = "ReturnUrl";
});

// Register HttpClient for calling the FastAPI service
builder.Services.AddHttpClient("FastApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8000/"); // Base URL of the FastAPI service
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Bind VNPAY settings from appsettings.json
builder.Services.Configure<VnPaySettings>(builder.Configuration.GetSection("VNPAY"));

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Define routes
app.MapControllerRoute(
    name: "cart",
    pattern: "gio-hang",
    defaults: new { controller = "ShoppingCart", action = "Index" });

app.MapControllerRoute(
    name: "checkout",
    pattern: "thanh-toan",
    defaults: new { controller = "ShoppingCart", action = "Check_out" });

app.MapControllerRoute(
    name: "deleteCart",
    pattern: "shoppingcart/Delete",
    defaults: new { controller = "ShoppingCart", action = "Delete" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
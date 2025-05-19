using Eshopp.Models;
using EShopp.Models.EF;
using EShopp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Eshopp.Controllers
{

    public class ShoppingCartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // Khai báo biến
        private readonly ApplicationDbContext _context; // Thay bằng DbContext của bạn

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor; // Inject qua constructor
            _context = context;
        }
        [HttpGet]
        public IActionResult Check_out()
        {
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

            if (cart.Items == null || !cart.Items.Any())
            {
                TempData["Message"] = "Bạn chưa có sản phẩm nào. Vui lòng quay lại trang sản phẩm để mua hàng.";
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [HttpPost]
        public IActionResult Check_out(string CustomerName, string Phone, string Address, string Email, int TypePayment)
        {
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

            if (cart.Items == null || !cart.Items.Any())
            {
                return Json(new { success = false, message = "Bạn chưa có sản phẩm nào!" });
            }

            try
            {
                string orderCode = "ORD-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var order = new Order
                {
                    Code = orderCode,
                    CustomerName = CustomerName,
                    Phone = Phone,
                    Address = Address,
                    TotalAmount = cart.Items.Sum(x => x.TotalPrice),
                    Quantity = cart.Items.Sum(x => x.Quantity),
                    TypePayment = TypePayment,
                    CreatedDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail>()
                };

                // Thêm OrderDetails
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        OrderId = order.Id // Cần thiết lập sau khi lưu order
                    };
                    order.OrderDetails.Add(orderDetail);
                }

                // Lưu order trước để lấy Id
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Cập nhật OrderId cho OrderDetails
                foreach (var detail in order.OrderDetails)
                {
                    detail.OrderId = order.Id; // Thiết lập khóa ngoại
                }
                _context.SaveChanges(); // Lưu lại để cập nhật OrderDetails

                // Xóa giỏ hàng
                _httpContextAccessor.HttpContext.Session.Remove("Cart");

                return Json(new { success = true, message = "Đặt hàng thành công!", redirectUrl = "/" });
            }
            catch (DbUpdateException ex)
            {
                // Ghi log chi tiết lỗi
                var innerException = ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = $"Lỗi khi đặt hàng: {innerException}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi khi đặt hàng: {ex.Message}" });
            }
        }



        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            Console.WriteLine("Giỏ hàng khi truy xuất: " + cartJson); // Debug
            var cart = string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            var response = new { success = false, message = "Lỗi hệ thống", code = -1, count = 0 };

            try
            {
                var product = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductCategory)
                    .FirstOrDefault(x => x.id == id);

                if (product == null)
                {
                    response = new { success = false, message = "Sản phẩm không tồn tại", code = -2, count = 0 };
                    return Json(response);
                }

                var cartJson = HttpContext.Session.GetString("Cart");
                var cart = string.IsNullOrEmpty(cartJson)
                    ? new ShoppingCart()
                    : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

                // Debug ProductImages
                Console.WriteLine($"[DEBUG] Số lượng ProductImages cho ProductId {id}: {product.ProductImages?.Count ?? 0}");
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    foreach (var img in product.ProductImages)
                    {
                        Console.WriteLine($"[DEBUG] Image: {img.Image}, IsDefault: {img.IsDefault}");
                    }
                }

                var productImage = product.ProductImages?.FirstOrDefault(x => x.IsDefault)?.Image ?? "/images/default-product.jpg";
                Console.WriteLine($"[DEBUG] Đường dẫn ảnh cho ProductId {id}: {productImage}");

                var cartItem = new ShoppingCart.ShoppingCartItem
                {
                    ProductId = product.id,
                    ProductName = product.Title,
                    Alias = product.Alias,
                    CategoryName = product.ProductCategory?.Title ?? "Không có danh mục",
                    ProductImage = productImage,
                    Price = product.PriceSale > 0 ? (decimal)product.PriceSale : (decimal)product.Price,
                    Quantity = quantity,
                    TotalPrice = (product.PriceSale > 0 ? (decimal)product.PriceSale : (decimal)product.Price) * quantity
                };

                // Gọi AddToCart
                cart.AddToCart(cartItem, quantity);

                // Cập nhật lại TotalPrice cho tất cả các mục trong giỏ
                foreach (var item in cart.Items)
                {
                    item.TotalPrice = item.Price * item.Quantity;
                    Console.WriteLine($"[DEBUG] ProductId: {item.ProductId}, Quantity: {item.Quantity}, TotalPrice: {item.TotalPrice}");
                }

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

                response = new { success = true, message = "Thêm vào giỏ hàng thành công", code = 1, count = (int)cart.GetTotalQuantity() };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi server: {ex.Message}\nStackTrace: {ex.StackTrace}");
                response = new { success = false, message = "Lỗi server: " + ex.Message, code = -99, count = 0 };
            }

            return Json(response);
        }

        [HttpGet]
        public IActionResult ShowCount()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonSerializer.Deserialize<ShoppingCart>(cartJson);

            return Json(new { count = (int)cart.GetTotalQuantity() }); // Dùng GetTotalQuantity thay vì Items.Count
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Xóa thất bại", count = 0 };

            try
            {
                Console.WriteLine($"[DEBUG] Nhận ID để xóa: {id}, Kiểu: {id.GetType()}");
                var cartJson = HttpContext.Session.GetString("Cart") ?? "null";
                Console.WriteLine($"[DEBUG] Giỏ hàng JSON: {cartJson}");

                if (string.IsNullOrEmpty(cartJson) || cartJson == "null")
                {
                    Console.WriteLine("[DEBUG] Giỏ hàng trống hoặc không tồn tại");
                    response = new { success = false, message = "Giỏ hàng trống", count = 0 };
                    return Json(response);
                }

                var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson);
                Console.WriteLine($"[DEBUG] Số lượng mục trong giỏ: {cart.Items.Count}");
                if (cart.Items == null)
                {
                    Console.WriteLine("[DEBUG] Items là null");
                    response = new { success = false, message = "Dữ liệu giỏ hàng bị lỗi", count = 0 };
                    return Json(response);
                }
                foreach (var item in cart.Items)
                {
                    Console.WriteLine($"[DEBUG] Item ProductId: {item.ProductId}, Name: {item.ProductName}");
                }

                var itemToRemove = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (itemToRemove != null)
                {
                    cart.Items.Remove(itemToRemove);
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
                    Console.WriteLine($"[DEBUG] Sau khi xóa, số lượng mục: {cart.Items.Count}");
                    response = new { success = true, message = "Xóa sản phẩm thành công", count = (int)cart.GetTotalQuantity() };
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Sản phẩm với ID {id} không tồn tại trong giỏ");
                    response = new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng", count = (int)cart.GetTotalQuantity() };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi server: {ex.Message}\nStackTrace: {ex.StackTrace}");
                response = new { success = false, message = "Lỗi server: " + ex.Message, count = 0 };
            }

            return Json(response);
        }


        [HttpPost]
        public IActionResult DeleteAll()
        {
            try
            {
                var cartJson = HttpContext.Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return Json(new { success = false, message = "Giỏ hàng đã trống", count = 0 });
                }

                var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson);
                if (cart == null || cart.Items == null)
                {
                    return Json(new { success = false, message = "Giỏ hàng lỗi", count = 0 });
                }

                cart.Items.Clear();
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

                return Json(new { success = true, message = "Đã xóa toàn bộ giỏ hàng", count = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message, count = 0 });
            }
        }


        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            try
            {
                // Kiểm tra số lượng hợp lệ
                if (quantity <= 0)
                {
                    return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });
                }

                // Lấy giỏ hàng từ Session
                var cartJson = HttpContext.Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                // Deserialize giỏ hàng
                var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();

                // Tìm sản phẩm cần cập nhật
                var itemToUpdate = cart.Items?.FirstOrDefault(x => x.ProductId == id);
                if (itemToUpdate == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
                }

                // Cập nhật số lượng và tính toán lại giá
                itemToUpdate.Quantity = quantity;
                itemToUpdate.TotalPrice = itemToUpdate.Price * quantity;

                // Lưu lại giỏ hàng vào Session
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

                // Tính toán các giá trị trả về
                var culture = CultureInfo.GetCultureInfo("vi-VN");
                return Json(new
                {
                    success = true,
                    message = "Cập nhật thành công",
                    itemTotal = itemToUpdate.TotalPrice.ToString("N0", culture),
                    total = cart.GetTotalPrice().ToString("N0", culture),
                    count = cart.GetTotalQuantity()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi hệ thống: " + ex.Message,
                    itemTotal = "0",
                    total = "0",
                    count = 0
                });
            }
        }
    }
}

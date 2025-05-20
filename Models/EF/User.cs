using Microsoft.AspNetCore.Identity;

namespace Eshopp.Models.EF
{
    public class ApplicationUser : IdentityUser
    {
        // Thêm các thuộc tính tùy chỉnh của bạn ở đây
        public string FullName { get; set; }
    }
}

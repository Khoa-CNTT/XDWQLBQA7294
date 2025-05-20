// Tạo file: ViewComponents/ChatboxViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eshopp.ViewComponents
{
    public class ChatboxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new Eshopp.ViewComponents.ChatboxModel();
            // Populate model properties
            return View(model);
        }
    }

    // Model cho chatbox (nếu cần)
    public class ChatboxModel
    {
        public List<string> Messages { get; set; }
    }
}
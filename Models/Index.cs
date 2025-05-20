using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshopp.Models
{
   
        public class IndexModel : PageModel
        {
            public string Message { get; set; }

            public void OnGet()
            {
                Message = "Welcome to the FastApiClientApp!";
            }
        }
    
}

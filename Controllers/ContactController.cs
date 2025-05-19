using Microsoft.AspNetCore.Mvc;

namespace Eshopp.Controllers
{
    [Route("lien-he")]
    public class ClientContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

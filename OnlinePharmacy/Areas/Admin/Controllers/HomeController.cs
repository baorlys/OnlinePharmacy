using Microsoft.AspNetCore.Mvc;

namespace OnlinePharmacy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("quan-ly")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

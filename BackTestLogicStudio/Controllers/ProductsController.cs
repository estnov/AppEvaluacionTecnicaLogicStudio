using Microsoft.AspNetCore.Mvc;

namespace BackTestLogicStudio.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

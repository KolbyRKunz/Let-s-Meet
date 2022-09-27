using Microsoft.AspNetCore.Mvc;

namespace Let_s_Meet.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

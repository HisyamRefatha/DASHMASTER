using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.WEB.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        public IActionResult Index()
        {
            ViewData["role"] = HttpContext.User.Claims.Where(d => d.Type == "role").FirstOrDefault().Value;
            return View();
        }
    }
}

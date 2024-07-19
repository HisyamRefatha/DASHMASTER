using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.WEB.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

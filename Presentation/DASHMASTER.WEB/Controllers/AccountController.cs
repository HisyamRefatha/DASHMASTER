using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.WEB.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}

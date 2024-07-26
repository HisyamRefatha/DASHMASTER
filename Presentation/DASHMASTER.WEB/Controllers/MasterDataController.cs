using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.WEB.Controllers
{
	public class MasterDataController : BaseController<MasterDataController>
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Product() 
		{
			return View();
		}

		public IActionResult Category() 
		{
			return View();
		}

		public IActionResult Inventory()
		{
			return View();
		}
	}
}

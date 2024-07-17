using DASHMASTER.WEB.Helper;
using DASHMASTER.WEB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using System.Security.Claims;

namespace DASHMASTER.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenHelper _token;
        private readonly IRestAPIHelper _apiRequest;
        private string BASE_URL = "";

        public AccountController(ILogger<AccountController> logger, ITokenHelper token, IRestAPIHelper apiRequest, IConfiguration configuration)
        {
            _logger = logger;
            _token = token;
            _apiRequest = apiRequest;
            BASE_URL = configuration.GetValue<string>("APIUrl");
        }

        public IActionResult Index()
        {
            return View();
        }

        #region login
        [HttpPost]
        public async Task<IActionResult> DoLogin(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _apiRequest.Login(model.Username, model.Password, BASE_URL);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>()
                    {
                         new Claim("user_id", result.Data.User.Id.ToString()),
                         new Claim("username", result.Data.User.Username),
                         new Claim(ClaimTypes.Email, result.Data.User.Email),
                         new Claim("role", result.Data.User.Role),
                         new Claim("token", result.Data.RawToken)
                    };
                    var identity = new ClaimsIdentity(claims, HelperClient.AUTHENTICATION_SCHEMA);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IssuedUtc = DateTime.Now.ToUniversalTime(),
                        ExpiresUtc = result.Data.ExpiredAt
                    };
                    await HttpContext.SignInAsync(HelperClient.AUTHENTICATION_SCHEMA, principal, properties);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                    ModelState.AddModelError("", result.Message + " : " + result.Description);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}

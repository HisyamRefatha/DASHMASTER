using DASHMASTER.WEB.Helper;
using DASHMASTER.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DASHMASTER.WEB.Controllers
{
    public class BaseController<T> : Controller
    {
        private ILogger<T> _loggerInstance;
        private IRestAPIHelper _apiRequestInstance;
        private ITokenHelper _tokenInstance;

        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected ITokenHelper _token => _tokenInstance ??= HttpContext.RequestServices.GetService<ITokenHelper>();
        protected IRestAPIHelper _apiRequest => _apiRequestInstance ??= HttpContext.RequestServices.GetService<IRestAPIHelper>();
        protected TokenModel _tokenData;


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var decode_token = _token.DecodeToken(HttpContext);
            if (!decode_token.Success)
            {
                context.Result = new RedirectResult($"~/Account/Login?redirect={Request.Path}");
                return;
            }
            List<string> exclude_path = new List<string>()
            {
                "/home/dashboard",
                "/home/refreshtoken",
                "/home/profile",
            };

            _tokenData = decode_token.Token;
            ViewBag.Token = decode_token.Token;
            ViewBag.Role = _tokenData.User.Role.ToString();
            await base.OnActionExecutionAsync(context, next);

            ViewBag.Path = Request.Path.Value;
        }
    }
}

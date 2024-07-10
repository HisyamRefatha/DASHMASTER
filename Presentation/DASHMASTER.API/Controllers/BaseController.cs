using AutoMapper;
using DASHMASTER.CORE.Attributes;
using DASHMASTER.CORE.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController<T> : Controller
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        private IMapper _mapperInstance;
        private ITokenHelper _tokenHelperInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
        protected ITokenHelper _tokenHelper => _tokenHelperInstance ??= HttpContext.RequestServices.GetService<ITokenHelper>();

        protected IActionResult Wrapper<TT>(TT response, object request)
        {
            var user = Token;
            if (user != null && user.User != null)
            {
                //_ = Task.Run(() => _mediator.Send(new AddApiLogRequest()
                //{
                //    IdUser = user.User.Id,
                //    Endpoint = Request.Path.Value,
                //    Request = request != null ? JsonConvert.SerializeObject(request) : "-",
                //    Response = response != null ? JsonConvert.SerializeObject(response) : "-",
                //}));
            }
            dynamic result = response!;
            int code = result.Code;
            return this.StatusCode(code, result);
        }
        protected IActionResult Wrapper<TT>(TT response)
        {
            dynamic result = response!;
            int code = result.Code;
            return this.StatusCode(code, result);
        }

        protected TokenObject Token
        {
            get
            {
                var result = new TokenObject();
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var requestKey))
                {
                    if (requestKey.Count > 0)
                    {
                        var key = requestKey.First().Split(' ');
                        if (key.Length == 2 && key[0].ToLower().Trim() == "bearer")
                        {
                            var decode_token = _tokenHelper.DecodeToken(key[1]);
                            if (decode_token.Succeeded)
                                result = decode_token.Data;
                        }
                    }
                }
                return result;
            }
        }

        protected string Inputer { get { return Token?.User?.Id.ToString() + "|" + Token?.User?.Username; } }
    }
}

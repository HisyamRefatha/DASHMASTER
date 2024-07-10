using DASHMASTER.CORE.Service.Identity.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.API.Controllers.V1.Identity
{
    public class UserController : BaseController<UserController>
    {
        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Wrapper(await _mediator.Send(request), request);
        }
    }
}

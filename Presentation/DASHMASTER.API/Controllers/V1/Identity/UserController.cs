using DASHMASTER.CORE.Service.Identity.Command;
using DASHMASTER.CORE.Service.Identity.Object;
using DASHMASTER.CORE.Service.Identity.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vleko.Result;

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

        
        [HttpPost(template: "add")]
        public async Task<IActionResult> AddUser([FromBody] UserRequest request)
        {
            var add_request = _mapper.Map<AddUserRequest>(request);
            add_request.Inputer = Token?.User?.Username;
            return Wrapper(await _mediator.Send(add_request));
        }
        
        [HttpPost(template: "list")]
        public async Task<IActionResult> ListUser([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<ListUserRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
    }
}

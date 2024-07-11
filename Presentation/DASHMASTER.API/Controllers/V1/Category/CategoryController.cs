using DASHMASTER.CORE.Service.Category.Command;
using DASHMASTER.CORE.Service.Category.Object;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DASHMASTER.API.Controllers.V1.Category
{
    public class CategoryController : BaseController<CategoryController>
    {
        [AllowAnonymous]
        [HttpPost(template: "add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            var add_request = _mapper.Map<AddCategoryRequest>(request);
            add_request.Inputer = Token?.User?.Username;
            return Wrapper(await _mediator.Send(add_request));
        }
    }
}

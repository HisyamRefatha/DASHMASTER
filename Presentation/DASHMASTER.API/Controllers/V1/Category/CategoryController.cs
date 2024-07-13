using DASHMASTER.CORE.Service.Category.Command;
using DASHMASTER.CORE.Service.Category.Object;
using DASHMASTER.CORE.Service.Category.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vleko.Result;

namespace DASHMASTER.API.Controllers.V1.Category
{
    public class CategoryController : BaseController<CategoryController>
    {
        
        [HttpPost(template: "add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            var add_request = _mapper.Map<AddCategoryRequest>(request);
            add_request.Inputer = Token?.User?.Username;
            return Wrapper(await _mediator.Send(add_request));
        }

        
        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<ListCategoryRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
    }
}

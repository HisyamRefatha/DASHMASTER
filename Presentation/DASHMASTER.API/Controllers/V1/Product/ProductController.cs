using DASHMASTER.CORE.Service.Product.Command;
using DASHMASTER.CORE.Service.Product.Object;
using DASHMASTER.CORE.Service.Product.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vleko.Result;

namespace DASHMASTER.API.Controllers.V1.Product
{
    public class ProductController : BaseController<ProductController>
    {
        [HttpPost(template: "add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest request)
        {
            var add_request = _mapper.Map<AddProductRequest>(request);
            add_request.Inputer = Token?.User?.Username;
            return Wrapper(await _mediator.Send(add_request));
        }

        
        [HttpPost(template: "list")]
        public async Task<IActionResult> list([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<ListProductRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
    }
}

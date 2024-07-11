using AutoMapper;
using DASHMASTER.CORE.Service.Category.Object;
using DASHMASTER.DATA;
using DASHMASTER.DATA.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vleko.DAL.Interface;
using Vleko.Result;

namespace DASHMASTER.CORE.Service.Category.Command
{
    #region request
    public class AddCategoryMapping : Profile
    {
        public AddCategoryMapping() 
        {
            CreateMap<AddCategoryRequest, CategoryRequest>().ReverseMap();
        }
    }

    public class AddCategoryRequest : CategoryRequest, IRequest<StatusResponse>
    {
        public string Inputer { get; set;}
    }
    #endregion
    internal class AddCategoryHandler : IRequestHandler<AddCategoryRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public AddCategoryHandler(ILogger<AddCategoryHandler> logger, IMapper mapper, IMediator mediator, IUnitOfWork<ApplicationDBContext> context)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<StatusResponse> Handle(AddCategoryRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new();
            try
            {
                string inp = "SYSTEM";

                if (!String.IsNullOrEmpty(request.Inputer))
                {
                    request.Inputer = inp;
                }

                var data = _mapper.Map<MstCategory>(request);
                data.CreateBy = inp;
                data.CreateDate = DateTime.Now;

                var add = await _context.AddSave(data);
                if (add.Success)
                {
                    result.OK();
                }
                else
                    result.BadRequest(add.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Failed Add Category", request);
                result.Error("Failed Add Category", ex.Message);
            }
            return result;
        }
    }
}

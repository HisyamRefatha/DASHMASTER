using AutoMapper;
using DASHMASTER.CORE.Service.Product.Object;
using DASHMASTER.DATA;
using DASHMASTER.DATA.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Vleko.DAL.Interface;
using Vleko.Result;

namespace DASHMASTER.CORE.Service.Product.Command
{
    #region request
    public class AddProductMapping : Profile
    {
        public AddProductMapping()
        {
            CreateMap<AddProductRequest, ProductRequest>().ReverseMap();
        }
    }

    public class AddProductRequest : ProductRequest, IRequest<StatusResponse>
    {
        public string Inputer { get; set;}
    }
    #endregion
    internal class AddProductHandler : IRequestHandler<AddProductRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public AddProductHandler(ILogger<AddProductHandler> logger, IMapper mapper, IMediator mediator, IUnitOfWork<ApplicationDBContext> context)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<StatusResponse> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new();
            try
            {
                string inp = "SYSTEM";
                var data_category = await _context.Entity<MstCategory>()
                    .Where(w => w.Id == request.CategoryId).FirstOrDefaultAsync();

                if(data_category == null) 
                {
                    result.NotFound($"Category Id {request.CategoryId} Not Found");
                    return result;
                }

                if (!String.IsNullOrEmpty(request.Inputer))
                {
                    request.Inputer = inp;
                }

                var data = _mapper.Map<MstProduct>(request);
                data.CreateBy = request.Inputer;
                data.CreateDate = DateTime.Now;
                data.CategoryId = data_category.Id;
                var add = await _context.AddSave(data);

                if (add.Success)
                {
                    result.OK();
                }
                else
                    result.BadRequest(add.Message);

            }catch(Exception ex) 
            {
                _logger.LogError(ex, "Failed Add Employee", request);
                result.Error("Failed Add Employee", ex.Message);
            }
            return result;
        }
    }
}

using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.CORE.Service.Identity.Object;
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

namespace DASHMASTER.CORE.Service.Identity.Command
{
    #region request
    public class AddUserMapping : Profile
    {
        public AddUserMapping() 
        {
            CreateMap<AddUserRequest, UserRequest>().ReverseMap();
        }
    }

    public class AddUserRequest : UserRequest, IRequest<StatusResponse>
    {
        public string Inputer { get; set; }
    }
    #endregion
    internal class AddUserHandler : IRequestHandler<AddUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public AddUserHandler(ILogger<AddUserHandler> logger, IMapper mapper, IMediator mediator, IUnitOfWork<ApplicationDBContext> context, IGeneralHelper helper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
            _helper = helper;
        }

        public async Task<StatusResponse> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new();
            try
            {
                string inp = "SYSTEM";
                if(!String.IsNullOrEmpty(request.Inputer))
                {
                    request.Inputer = inp;
                }

                var data = _mapper.Map<MstUser>(request);
                data.CreateBy = inp;
                data.CreateDate = DateTime.Now;
                data.Password = _helper.PasswordEncrypt(request.Password);

                var add = await _context.AddSave(data);
                if(add.Success)
                {
                    result.OK();
                }
                else
                    result.BadRequest(add.Message);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Failed Add User", request);
                result.Error("Failed Add User", ex.Message);
            }
            return result;
        }
    }
}

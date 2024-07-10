using DASHMASTER.CORE.Attributes;
using DASHMASTER.CORE.Helper;
using DASHMASTER.CORE.Service.Identity.Object;
using DASHMASTER.DATA;
using DASHMASTER.DATA.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Vleko.DAL.Interface;
using Vleko.Result;

namespace DASHMASTER.CORE.Service.Identity.Command
{
    #region request
    public class LoginRequest : IRequest<ObjectResponse<TokenObject>>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    #endregion

    internal class LoginUserHandler : IRequestHandler<LoginRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly ITokenHelper _token;
        private readonly IUnitOfWork<ApplicationDBContext> _context;

        public LoginUserHandler(ILogger<LoginUserHandler> logger, IMediator mediator, IGeneralHelper helper, ITokenHelper token, IUnitOfWork<ApplicationDBContext> context)
        {
            _logger = logger;
            _mediator = mediator;
            _helper = helper;
            _token = token;
            _context = context;
        }

        public async Task<ObjectResponse<TokenObject>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<TokenObject> result = new();
            try
            {
                string _hash = _helper.PasswordEncrypt(request.Password);
                var user = await _context.Entity<MstUser>()
                    .Where(w => w.Username.ToLower() == request.Username.ToLower()).FirstOrDefaultAsync();
                if (user != null)
                {
                    if(user.Password != _hash)
                    {
                        result.Forbidden($"Failed login pleas check again your password!");
                        return result;
                    }

                    var generateToken = await _token.GenerateToken(new TokenUserObject()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role
                    });

                    if (generateToken.Succeeded)
                    {
                        result.Data = generateToken.Data;
                        user.Token = result.Data.RefreshToken;

                        var update = await _context.UpdateSave(user);
                        if (update.Success)
                            result.OK();
                        else
                            result.BadRequest(update.Message);
                    }
                    else
                        result = generateToken;
                }
                else
                    result.NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed Login User", request);
                result.Error("Failed Add User", ex.Message);
            }
            return result;
        }
    }
}

using DASHMASTER.WEB.Models;
using Microsoft.AspNetCore.Authentication;
using SixLabors.ImageSharp;
using System.Security.Claims;

namespace DASHMASTER.WEB.Helper
{
    public interface ITokenHelper
    {
        (ClaimsPrincipal principal, AuthenticationProperties properties) CreateToken(TokenModel request);
        (bool Success, TokenModel Token) DecodeToken(HttpContext context);
    }
    public class TokenHelper : ITokenHelper
    {
        private readonly ILogger _logger;
        private string API_URL;

        public TokenHelper(IConfiguration configuration, ILogger<TokenHelper> logger)
        {
            _logger = logger;
            API_URL = configuration.GetValue<string>("APIUrl");
        }
        #region Create
        public (ClaimsPrincipal principal, AuthenticationProperties properties) CreateToken(TokenModel request)
        {
            try
            {
                var claims = new List<Claim>()
                    {
                        new Claim("user_id", request.User.Id.ToString()),
                        new Claim("username", request.User.Username),
                        new Claim(ClaimTypes.Email, request.User.Email),
                        new Claim(ClaimTypes.Role, request.User.Role),
                        new Claim("token", request.RawToken)
                    };

                var identity = new ClaimsIdentity(claims, HelperClient.AUTHENTICATION_SCHEMA);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.Now.ToUniversalTime(),
                    ExpiresUtc = request.ExpiredAt
                };
                return (principal, properties);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
        #endregion

        #region Decode
        public (bool Success, TokenModel Token) DecodeToken(HttpContext context)
        {
            try
            {
                var claims = context.User.Identities?.First().Claims.ToList();
                if (claims != null && claims.Count() > 0)
                {
                    var token = new TokenModel()
                    {
                        RawToken = claims?.FirstOrDefault(x => x.Type.Equals("token"))?.Value,
                        User = new TokenUserModel()
                        {
                            Id = Guid.Parse(claims?.FirstOrDefault(x => x.Type.Equals("user_id"))?.Value),
                            Username = claims?.FirstOrDefault(x => x.Type.Equals("username"))?.Value,
                            Email = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value,
                            Role = claims?.FirstOrDefault(x => x.Type.Equals("role"))?.Value
                        },
                        BaseApiUrl = API_URL
                    };
                    return (true, token);
                }
                else
                    return (false, null);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }
        #endregion
    }
}

using DASHMASTER.WEB.Models;
using System.Security.Claims;

namespace DASHMASTER.WEB.Helper
{
    public class HelperClient
    {
        public const string AUTHENTICATION_SCHEMA = "DASHMASTER";
        public const string PAGE_COOKIE = "DASHMASTER.Page";

        private readonly ILogger _logger;
        private string API_URL;
        public HelperClient(IConfiguration configuration,
            ILogger<HelperClient> logger
            )
        {
            API_URL = configuration.GetValue<string>("APIBaseUrl");
            _logger = logger;
        }


        public static (bool Success, TokenModel Result) DecodeToken(HttpContext context)
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
                            Role = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value
                        },
                        //BaseApiUrl = API_URL
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
    }
}

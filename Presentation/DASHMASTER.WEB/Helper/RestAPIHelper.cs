using DASHMASTER.WEB.Models;
using Newtonsoft.Json;
using System.Text;
using Vleko.Result;

namespace DASHMASTER.WEB.Helper
{
    public interface IRestAPIHelper
    {
        Task<ObjectResponse<TokenModel>> Login(string username, string password, string baseUrl);
        Task<dynamic> DoRequest(string url, string method, string body, bool isAnnonymous);
    }
    public class RestAPIHelper : IRestAPIHelper
    {
        public const string COOKIES_TOKEN = "DASHMASTER.API.TOKEN";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RestAPIHelper> _logger;
        private string BASE_URL = "";
        public string TokenApp = "";

        public RestAPIHelper(IHttpContextAccessor httpContextAccessor, ILogger<RestAPIHelper> logger, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            BASE_URL = configuration.GetValue<string>("APIUrl");
        }

        public async Task<ObjectResponse<TokenModel>> Login(string username, string password, string baseUrl)
        {
            //string url = $"{BASE_URL}/v1/User/login";
            return await DoRequest<ObjectResponse<TokenModel>>($"{baseUrl}/v1/User/login", HttpMethod.Post, new { username, password }, true);
        }

        #region Do Request Utility
        private async Task<ObjectResponse<string>> Request(string url, HttpMethod httpMethod, string json_body, bool isAnnonymous)
        {
            ObjectResponse<string> result = new ObjectResponse<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var token = _httpContextAccessor.HttpContext.Request.Cookies[COOKIES_TOKEN];

                    if (!isAnnonymous && token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
                    }

                    var request = new HttpRequestMessage(httpMethod, $"{url}");

                    if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && !string.IsNullOrWhiteSpace(json_body))
                    {
                        request.Content = new StringContent(json_body, Encoding.UTF8, "application/json");
                    }

                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        result.BadRequest("Something Went Wrong!");
                        result.Code = (int)response.StatusCode;
                        result.Message = response.StatusCode.ToString();
                    }
                    else
                    {
                        result.Data = content;
                        result.OK();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Do Request", url);
                result.Error("Failed Request", ex.Message);
            }
            return result;
        }

        private async Task<T> DoRequest<T>(string url, HttpMethod method, object body, bool isAnnonymous) where T : class
        {
            var request = await Request(url, method, body != null ? JsonConvert.SerializeObject(body) : null, isAnnonymous);
            if (request.Succeeded)
            {
                return JsonConvert.DeserializeObject<T>(request.Data);
            }
            else
            {
                return null;
            }
        }
        public async Task<dynamic> DoRequest(string url, string method, string body, bool isAnnonymous)
        {
            HttpMethod _method = new HttpMethod(method);
            //string reqBody = "";
            //if (!string.IsNullOrWhiteSpace(body))
            //    reqBody = JsonConvert.DeserializeObject<dynamic>(body);

            var request = await Request($"{BASE_URL}/{url}", _method, body, isAnnonymous);
            if (request.Succeeded)
            {
                return JsonConvert.DeserializeObject<dynamic>(request.Data);
            }
            else
            {
                return new
                {
                    request.Code,
                    request.Succeeded,
                    request.Message,
                    request.Description
                };
            }
        }
        #endregion
    }
}

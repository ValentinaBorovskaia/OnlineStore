using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CartingService.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost, Route("login")]
        public async Task<string> Login(UserModel model)
        {
            var authenticationUrl = configuration["AuthEndPoint"];

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(authenticationUrl, model);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenObject = JObject.Parse(responseContent);
                    var token = tokenObject["token"].ToString();

                    return token;
                }
            }
            return null;
        }
    }
}

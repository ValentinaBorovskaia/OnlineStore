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
       
        public LoginController() 
        {
          
        }

        [HttpPost, Route("login")]
        public async Task<string> Login(UserModel model)
        {
            var authenticationUrl = "https://localhost:7103/api/auth/login"; // Replace with the authentication endpoint of App A
            var loginModel = new { Username = "123", Password = "123" };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(authenticationUrl, loginModel);
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

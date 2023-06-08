using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CartingService.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService tokenService;
        public LoginController(ITokenService tokenService) 
        {
            this.tokenService = tokenService;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(UserModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Login) ||
                string.IsNullOrEmpty(model.Password))
                    return BadRequest("Username and/or Password not specified");
              var result =  await tokenService.GetToken(model.Login);
                return Ok(result);
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }
    }
}

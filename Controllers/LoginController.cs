using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testapi.Repository.Service;

namespace testapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtTokenService tokenService;
        public LoginController(JwtTokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Model.Login login)
        {
            try
            {
                if (login != null)
                {
                    var users =  tokenService.AuthenticateUser(login);

                    var Token = tokenService.GenerateJSONWebToken(users);

                    return Ok(new
                    {
                        message= "success",
                        token = Token
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "error",
                        token = ""
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

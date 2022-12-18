using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testapi.DataLayer;
using testapi.Model;

namespace testapi.Repository.Service
{
    public class JwtTokenService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext applicationDbContext;

        public JwtTokenService(IConfiguration configuration,ApplicationDbContext applicationDbContext)
        {
            this.configuration = configuration;
            this.applicationDbContext = applicationDbContext;
        }
        public string GenerateJSONWebToken(Model.Login login)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", login.UserName),
                        new Claim("Password", login.Password),
                    };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
              configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Model.Login AuthenticateUser(Model.Login login)
        {
            try
            {
                var users = applicationDbContext.users.Where(s => s.UserName == login.UserName || s.Password == login.Password).FirstOrDefault();

                if (users != null)
                {
                    Login logins = new()
                    {
                        UserName = users.UserName,
                        Password = users.Password
                    };
                    return logins;
                }
                else
                {
                    return null;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}

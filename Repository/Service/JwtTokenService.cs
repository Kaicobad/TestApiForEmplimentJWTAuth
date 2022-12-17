using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testapi.DataLayer;

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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
              configuration["Jwt:Issuer"],
              claims: null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Model.Login AuthenticateUser(Model.Login login)
        {
           var users = applicationDbContext.users.Select(new );
        }
    }
}

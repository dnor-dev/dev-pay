using BCrypt.Net;
using dev_pay.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace dev_pay
{
    public class Utility: IUtility
    {
        private readonly IConfiguration config;
        public Utility(IConfiguration _configuration)
        {
            config = _configuration;
        }

        public string hashPassword(string? password)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);
            return hashed;
        }

        public bool comparePasswords(string? InputPassword, string? DBPassword)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(InputPassword, DBPassword);
            return verified;
        }

        public string GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: config["JWT:Issuer"],
                    audience: config["JWT:Audience"],
                    expires: DateTime.Now.AddHours(200),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            var registeredToken = new JwtSecurityTokenHandler().WriteToken(token);
            return registeredToken;
        }

        public ByteArrayContent reqData(dynamic obj)
        {
            string serializedData = JsonConvert.SerializeObject(obj);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);
            ByteArrayContent data = new ByteArrayContent(buffer);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return data;
        }
    }
}

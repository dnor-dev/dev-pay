using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace dev_pay.Interfaces
{
    public interface IUtility
    {
        public string hashPassword(string? Password);

        public bool comparePasswords(string? InputPassword, string? DBPassword);

        public string GetToken(List<Claim> authClaims);
    }
}

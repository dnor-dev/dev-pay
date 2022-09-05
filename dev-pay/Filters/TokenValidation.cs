using dev_pay.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace dev_pay.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TokenValidation : Attribute, IAsyncActionFilter
    {     
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var token = context.HttpContext.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                context.HttpContext.Items["userEmail"] = jsonToken?.Payload?.Claims?.Where(claim => claim.Type.Contains("emailaddress")).FirstOrDefault()?.Value;

                await next();
            }
            catch (Exception? e) {
                throw new SystemException("Unauthorized");
            }
        }
    }
}

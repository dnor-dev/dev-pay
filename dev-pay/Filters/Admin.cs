using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace dev_pay.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class Admin : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var roles = jsonToken?.Payload?.Claims?.Where(claim => claim.Type.Contains("role"));
            var userRoles = new List<string>();
            foreach (var role in roles)
            {
                userRoles.Add(role.Value);
            }

            if (!userRoles.Contains("admin"))
            {
                throw new ApplicationException("You're not admin");
            }
            await next();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using dev_pay.Models;
using dev_pay.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace dev_pay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class customer : ControllerBase
    {
        private readonly ICustomerService CustomerService;
        private readonly ICustomerRepository customerRepository;
        private readonly IUtility utils;

        public customer(ICustomerService customerService, ICustomerRepository _customerRepository, IUtility _utils)
        {
            CustomerService = customerService;
            customerRepository = _customerRepository;
            utils = _utils;
        }


        [HttpPost("signup")]
        public async Task<ActionResult> Signup([FromBody] SignupModel model)
        {
            var userExist = await customerRepository.GetAsync(model.email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Failed", message = "User exists!"});
            }

            var res = await CustomerService.Create(model);
            if (res == null)
            {
                return BadRequest();
            }

            return Ok(new Response
            {
                message = "User created",
                status = "Success"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login([FromBody] LoginModel model)
        {
            var user = await customerRepository.GetAsync(model.email);

            if (user == null || !utils.comparePasswords(model.password, user.password))
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { message = "Invalid Credentials", status = "Failed" });
            }

            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user?.email),
                new Claim(ClaimTypes.Name, user?.first_name),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(JwtRegisteredClaimNames.Jti, user?.customer_code),
            };

            return Ok(new
            {
                id = user.id,
                first_name = user.first_name,
                last_name = user.last_name,
                email = user.email,
                customer_code = user.customer_code,
                phone = user.phone,
                integration = user.integration,
                identified = user.identified,
                identifications = user.identifications,
                token = utils.GetToken(authClaims)
            });
        }

        // POST api/<Customer>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Customer>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Customer>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

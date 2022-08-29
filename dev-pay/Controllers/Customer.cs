using Microsoft.AspNetCore.Mvc;
using dev_pay.Models;
using dev_pay.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using dev_pay.Filters;
using dev_pay.Models.Transaction;
using dev_pay.Models.Admin;
using dev_pay.Models.Customer.Requests;

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
        public async Task<ActionResult> Login([FromBody] LoginModel model)
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
                status = "Success",
                token = utils.GetToken(authClaims)
            });
        }

        [TokenValidation]
        [HttpPost]
        [Route("/api/admin")]
        public async Task<ActionResult> MakeTempAdmin([FromBody] AdminLogin model)
        {
            var user = await customerRepository.GetAsync(model.email);
            if (user is null)
            {
                throw new KeyNotFoundException("User not found");
            }
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user?.email), 
                new Claim(ClaimTypes.Name, user?.first_name),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(JwtRegisteredClaimNames.Jti, user?.customer_code),
            };

            return Ok(new
            {
                status = "Success",
                token = utils.GetToken(authClaims)
            });
        }


        [Authorize]
        [HttpPost("validate/{email}")]
        public async Task<ActionResult<Response>> Validate(string email, [FromBody] ValidateModel model)
        {
            var user = await customerRepository.GetAsync(email);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var res = await CustomerService.ValidateCustomer(user, model);
            if (res.status == "false")
            {
                throw new ApplicationException(res.message);
            }

            return Ok(res);
        }

        [TokenValidation]
        [HttpGet]
        public async Task<ActionResult> GetCustomer()
        {
            var userEmail = Request.HttpContext.Items["userEmail"]?.ToString();
            var user = await CustomerService.GetCustomerFullDetails(userEmail);
            return Ok(user);
        }

        [TokenValidation]
        [HttpPut]
        public async Task<ActionResult> UpdatePhoneNumber([FromBody] UpdatePhone model)
        {
            var userEmail = Request.HttpContext.Items["userEmail"]?.ToString();
            var res = await CustomerService.UpdatePhone(userEmail, model);
            return Ok(res);
        }

        [TokenValidation]
        [HttpPost("transactions")]
        public async Task<ActionResult> BuyAirtime([FromBody] InitiateTransactionModel model)
        {
            var res = await CustomerService.InitiateTransaction(model);
            return Ok(res);
        }

        [TokenValidation]
        [Admin]
        [HttpGet("transactions")]
        public async Task<ActionResult> AllTransactions()
        {
            var data = await CustomerService.GetAllTransactions();
            return Ok(new
            {
                status = "Success",
                data
            });
        }

        [TokenValidation]
        [Admin]
        [HttpGet("transactions/{id}")]
        public async Task<ActionResult> GetTransaction(int id)
        {
            var data = await CustomerService.GetSingleTransaction(id);
            return Ok(data);
        }

        [TokenValidation]
        [Admin]
        [HttpGet("transactions/verify/{reference}")]
        public async Task<ActionResult> VerifyTransaction(string reference)
        {
            var data = await CustomerService.VerifyCustomerTransaction(reference);
            return Ok(data);
        }
    }
}

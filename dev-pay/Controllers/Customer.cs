using dev_pay.Filters;
using dev_pay.Interfaces;
using dev_pay.Models;
using dev_pay.Models.Admin;
using dev_pay.Models.Customer.Requests;
using dev_pay.Models.Transaction;
using dev_pay.Models.VTU;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace dev_pay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class customer : ControllerBase
    {
        private readonly IPaystackService PaystackService;
        private readonly IVTUService VTU;
        private readonly ICustomerRepository customerRepository;
        private readonly IUtility utils;

        public customer(IPaystackService _paystackService, ICustomerRepository _customerRepository, IUtility _utils, IVTUService _vtu)
        {
            PaystackService = _paystackService;
            VTU = _vtu;
            customerRepository = _customerRepository;
            utils = _utils;
        }


        [HttpPost("signup")]
        public async Task<ActionResult> Signup([FromBody] SignupModel model)
        {
            var userExist = await customerRepository.GetAsync(model.email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Failed", message = "User exists!" });
            }

            var res = await PaystackService.Create(model);
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


        [TokenValidation]
        [HttpPost("validate/{email}")]
        public async Task<ActionResult<Response>> Validate(string email, [FromBody] ValidateModel model)
        {
            var user = await customerRepository.GetAsync(email);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var res = await PaystackService.ValidateCustomer(user, model);
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
            var user = await PaystackService.GetCustomerFullDetails(userEmail);
            return Ok(user);
        }

        [TokenValidation]
        [HttpPut("update-phone")]
        public async Task<ActionResult> UpdatePhoneNumber([FromBody] UpdatePhone model)
        {
            var userEmail = Request.HttpContext.Items["userEmail"]?.ToString();
            var res = await PaystackService.UpdatePhone(userEmail, model);
            return Ok(res);
        }


        [TokenValidation]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var user = await customerRepository.GetAsync(Request.HttpContext.Items["userEmail"]?.ToString());

            if (user is null)
            {
                throw new KeyNotFoundException("Customer does not exist");
            }

            if (!utils.comparePasswords(model.OldPassword, user.password))
            {
                throw new ApplicationException("Invalid Password");
            }

            await customerRepository.UpdatePassword(user.email, model.NewPassword);
            return Ok(new Response
            {
                status = "Success",
                message = "Password Updated"
            });
        }

        [TokenValidation]
        [HttpPost("transaction")]
        public async Task<ActionResult> InitializeTransaction([FromBody] InitiateTransactionModel model)
        {
            var res = await PaystackService.InitiateTransaction(model);
            return Ok(res);
        }

        [TokenValidation]
        [HttpPost("transactions/airtime/{reference}")]
        public async Task<ActionResult<AirtimeResponseModel>> Airtime(string reference, [FromBody] AirtimeRequestModel model)
        {
            var res = await PaystackService.AirtimeTransaction(reference, model);
            return Ok(res);
        }

        [TokenValidation]
        [HttpPost("verify-vtu")]
        public async Task<ActionResult> VerifyVTUService([FromBody] VerifyVTUServiceModel model)
        {
            var res = await VTU.Verify(model);
            return Ok(res);
        }

        [TokenValidation]
        [HttpPost("transactions/cable-tv/{reference}")]
        public async Task<ActionResult> CableTV(string reference, [FromBody] CableTVModel model)
        {
            var res = await PaystackService.CableTvTransaction(reference, model);
            return Ok(res);
        }

        [TokenValidation]
        [HttpPost("transactions/electricity/{reference}")]
        public async Task<ActionResult> Electricity(string reference, PayElectricityModel model)
        {
            var res = await PaystackService.ElectricityTransaction(reference, model);
            return Ok(res);
        }

        [TokenValidation]
        [Admin]
        [HttpGet("transactions")]
        public async Task<ActionResult> AllTransactions()
        {
            var data = await PaystackService.GetAllTransactions();
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
            var data = await PaystackService.GetSingleTransaction(id);
            return Ok(data);
        }

        [TokenValidation]
        [Admin]
        [HttpGet("transactions/verify/{reference}")]
        public async Task<ActionResult> VerifyTransaction(string reference)
        {
            var data = await PaystackService.VerifyCustomerTransaction(reference);
            return Ok(data);
        }
    }
}

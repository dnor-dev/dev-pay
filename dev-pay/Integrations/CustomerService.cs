
using dev_pay.Interfaces;
using dev_pay.Models;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using dev_pay.Models.Customer;

namespace dev_pay.Integrations
{
    public class CustomerService: ICustomerService
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;
        private readonly IUtility utils;
        private readonly ICustomerRepository CustomerRepository;

        public CustomerService(HttpClient _client, IConfiguration _config, ICustomerRepository customerRepository, IUtility _utils)
        {
            client = _client;
            CustomerRepository = customerRepository;
            config = _config;
            utils = _utils;
            client.BaseAddress = new Uri("https://api.paystack.co");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + config["SECRET_KEY"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Customer> Create(SignupModel customer)
        {
            var PaystackModel = new PaystackModel
            {
                email = customer.email,
                first_name = customer.first_name,
                last_name = customer.last_name,
                phone = customer.phone,
            };
            var customerData = utils.reqData(PaystackModel);
            var res = await client.PostAsync("customer", customerData);
            if (!res.IsSuccessStatusCode)
            {
            return null;
            }
            var obj = await res.Content.ReadAsAsync<CustomerCreateResponse>();
            if (obj.message != "Customer created" || obj.status != true)
            {
                return null;
            }

            Customer createdCustomer = new Customer
            {
                id = obj.data.id,
                first_name = obj.data.first_name,
                last_name = obj.data.last_name,
                customer_code = obj.data.customer_code,
                email = obj.data.email,
                phone = obj.data.phone,
                integration = obj.data.integration,
                identified = obj.data.identified,
                identifications = obj.data.identifications,
                password = utils.hashPassword(customer.password),
            };
            Customer entity = await CustomerRepository.AddAsync(createdCustomer);
            return entity;
        }

        public async Task<Response> ValidateCustomer(Customer user, ValidateModel model)
        {
            ByteArrayContent validateData = utils.reqData(model);
            var res = await client.PostAsync($"customer/{user.customer_code}/identification", validateData);
            if (!res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsAsync<Response>();
            }
            await CustomerRepository.UpdateAsync(user.email, user);

            var obj = await res.Content.ReadAsAsync<Response>();
            return obj;
        }

        public async Task<GetCustomerResponse> GetCustomerFullDetails(string? email)
        {
            var res = await client.GetAsync($"customer/{email}");
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong with your request");
            }

            var obj = await res.Content.ReadAsAsync<GetCustomerResponse>();
            
            return obj;
        }
    }
}

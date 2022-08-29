
using dev_pay.Interfaces;
using dev_pay.Models;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using dev_pay.Models.Customer;
using dev_pay.Models.Transaction;
using dev_pay.Models.Transaction.Transactions;
using dev_pay.Models.Customer.Requests;

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
                throw new ApplicationException("Something went wrong");
            }
            var obj = await res.Content.ReadAsAsync<CustomerCreateResponse>();
            if (obj.message != "Customer created" || obj.status != true)
            {
                throw new ApplicationException("Something went wrong");
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

        public async Task<GetCustomerResponse> UpdatePhone(string? email, UpdatePhone? model)
        {
            var customer = await CustomerRepository.GetAsync(email);
            if (customer is null)
            {
                throw new KeyNotFoundException("Customer does not exist");
            }
            var data = utils.reqData(model);
            var res = await client.PutAsync($"customer/{customer.customer_code}", data);
            var obj = await res.Content.ReadAsAsync<GetCustomerResponse>();
            await CustomerRepository.UpdatePhone(email, model);
            return obj;
        }

        public async Task<InitiateTransactionResponse> InitiateTransaction(InitiateTransactionModel model)
        {
            var userExist = await CustomerRepository.GetAsync(model.email);

            if (userExist is null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            if (Int32.Parse(model.amount) < 100)
            {
                throw new ApplicationException("amount should be greater than 100");
            }

            var res = await client.PostAsync("transaction/initialize", utils.reqData(model));
            if (!res.IsSuccessStatusCode)
            {
                throw new ApplicationException("Something went wrong");
            }

            var obj = await res.Content.ReadAsAsync<InitiateTransactionResponse>();
            return obj;
        }

        public async Task<Transactions> GetAllTransactions()
        {
            var res = await client.GetAsync("transaction");
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong");
            }

            var obj = await res.Content.ReadAsAsync<Transactions>();
            return obj;
        }

        public async Task<Transaction> GetSingleTransaction(int? id)
        {
            var res = await client.GetAsync($"transaction/{id}");
            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<Transaction>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<Transaction>();
            return obj;
        }

        public async Task<VerifyTransactionModel> VerifyCustomerTransaction(string reference)
        {
            var res = await client.GetAsync($"transaction/verify/{reference}");
            if (!res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsAsync<VerifyTransactionModel>();
                throw new ApplicationException(response.message);
            }

            var obj = await res.Content.ReadAsAsync<VerifyTransactionModel>();
            return obj;
        }
    }
}

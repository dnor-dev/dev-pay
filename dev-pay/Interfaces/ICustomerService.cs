using dev_pay.Models;
using dev_pay.Models.Customer;

namespace dev_pay.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(SignupModel customer);
        Task<Response> ValidateCustomer(Customer user, ValidateModel model);
        Task<GetCustomerResponse> GetCustomerFullDetails(string? email);
    }
}

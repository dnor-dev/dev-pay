using dev_pay.Models;

namespace dev_pay.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(SignupModel customer);
    }
}

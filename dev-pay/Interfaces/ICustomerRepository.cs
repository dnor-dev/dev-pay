using dev_pay.Models.Customer;
using dev_pay.Models.Customer.Requests;

namespace dev_pay.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);

        Task<Customer> GetAsync(string? email);

        Task<Customer> UpdateAsync(string email, Customer customer);
        Task<Customer> UpdatePhone(string email, UpdatePhone phone);
    }
}

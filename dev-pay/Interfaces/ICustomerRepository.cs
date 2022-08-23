using dev_pay.Models.Customer;

namespace dev_pay.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);

        Task<Customer> GetAsync(string? email);

        Task<Customer> UpdateAsync(string email, Customer customer);
    }
}

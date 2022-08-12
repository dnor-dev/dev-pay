using dev_pay.Models;

namespace dev_pay.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);

        Task<Customer> GetAsync(string? email);
    }
}

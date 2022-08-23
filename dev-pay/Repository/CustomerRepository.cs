using dev_pay.DB;
using dev_pay.Interfaces;
using dev_pay.Models;
using dev_pay.Models.Customer;
using Microsoft.EntityFrameworkCore;

namespace dev_pay.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext CustomerDb;

        public CustomerRepository(CustomerContext customerDb)
        {
            CustomerDb = customerDb;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            var transaction = await CustomerDb.Database.BeginTransactionAsync();
            await CustomerDb.Customers.AddAsync(customer);
            await CustomerDb.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Customers ON;");
            await CustomerDb.SaveChangesAsync();
            await CustomerDb.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Customers OFF;");
            await transaction.CommitAsync();
            return customer;
        }

        public async Task<Customer> GetAsync(string? email)
        {
            var customer = await CustomerDb.Customers.Where((x) => x.email == email).FirstOrDefaultAsync();
            if (customer is null)
            {
                return null;
            }
            return customer;
        }

        public async Task<Customer> UpdateAsync(string email, Customer customer)
        {
            var oldCustomer = await GetAsync(email);
            if (oldCustomer is null)
            {
                throw new KeyNotFoundException("Customer not found");
            }
            CustomerDb.Entry(oldCustomer).CurrentValues.SetValues(customer);
            await CustomerDb.SaveChangesAsync();
            return oldCustomer;
        }
    }
}

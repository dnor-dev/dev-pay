using dev_pay.Models.Customer;
using dev_pay.Models.Transaction;
using Microsoft.EntityFrameworkCore;

namespace dev_pay.DB
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<TransactionDBModel> Transactions { get; set; }
    }
}

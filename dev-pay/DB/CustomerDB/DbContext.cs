using dev_pay.Models;
using Microsoft.EntityFrameworkCore;

namespace dev_pay.DB
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}

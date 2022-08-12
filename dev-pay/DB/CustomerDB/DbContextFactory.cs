using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace dev_pay.DB
{
    public class DbContextFactory: IDesignTimeDbContextFactory<CustomerContext>
    {
        public CustomerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=dotnetDB;Trusted_Connection=true");

            return new CustomerContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MVCEmployee.Models.Cascade;
using MVCEmployee.Models.Excel;

namespace MVCEmployee.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }    
        public DbSet<District> Districts { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}

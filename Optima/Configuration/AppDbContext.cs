using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Optima.Entity.Employee;

namespace Optima.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    DbSet<Employee> Employees { get; set; }
    
    
    
   
}
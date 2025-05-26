using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductManagement.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Sử dụng connection string cho SQL Server Express
            optionsBuilder.UseSqlServer("Server=LAPTOP-ABBENG8N\\SQLEXPRESS;Database=ProductManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false");
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

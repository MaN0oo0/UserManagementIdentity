using Microsoft.EntityFrameworkCore;
using UserManagmentWithIdentity.Data;

namespace UserManagmentWithIdentity.Helpers
{
    public class DbContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public DbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(_options);
        }
    }
}

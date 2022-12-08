using Microsoft.EntityFrameworkCore;
using testapi.Model;

namespace testapi.DataLayer
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Product> products { get; set; }
        public DbSet<User> users { get; set; }
    }
}

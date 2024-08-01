using LoginRegisterApi.Models;
using LoginRegisterApi_Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserDetail> Users { get; set; }

        public DbSet<LoginDetail> LoginDetails { get; set; }

    }
}

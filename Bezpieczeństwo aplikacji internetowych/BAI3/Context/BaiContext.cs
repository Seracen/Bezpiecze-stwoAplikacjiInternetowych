using BAI3.Models;
using Microsoft.EntityFrameworkCore;

namespace BAI3.Context
{
    public class BaiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LogowanieZdarzen> LogowanieZdarzens { get; set; }
        public DbSet<LoginAttepmts> LoginAttempts { get; set; }
        public DbSet<FragmentalPasswordSchema> FragmentalPasswordSchemas { get; set; }
        public BaiContext(DbContextOptions<BaiContext> options) : base(options)
        {

        }
        
    }
}

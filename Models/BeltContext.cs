using Microsoft.EntityFrameworkCore;

namespace belt.Models {
    public class BeltContext : DbContext {
        public BeltContext(DbContextOptions options) : base(options) {}
        public DbSet<User> users {get;set;}
        public DbSet<Occasion> occasions {get;set;}

        public DbSet<Association> associations {get;set;}
    }
}
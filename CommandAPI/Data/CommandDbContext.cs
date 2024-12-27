using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Data
{
    public class CommandDbContext : DbContext
    {
        public CommandDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Command> Commands => Set<Command>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Command>().HasKey(c=> c.Id);
            modelBuilder.Entity<Command>().Property(c=> c.HowTo).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Command>().Property(c => c.Platform).IsRequired();
            modelBuilder.Entity<Command>().Property(c => c.CommandLine).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}

using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Shared.api.Context
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Rol>().HasData(new Rol
            {
                Id = 1,
                Nombre = "admin"
            });
            modelBuilder.Entity<Rol>().HasData(new Rol
            {
                Id = 2,
                Nombre = "cliente"
            });

        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
    }
}

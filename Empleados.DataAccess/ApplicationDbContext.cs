using Empleados.Models;
using Microsoft.EntityFrameworkCore;

namespace Empleados.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Empleado>()
                .Property(e => e.Salario)
                .HasColumnType("decimal(10,2)");
        }
    }
}
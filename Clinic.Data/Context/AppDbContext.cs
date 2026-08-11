using Clinic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReserveRecord> ReserveRecords { get; set; }

        //TODO: Invoke Rules
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk is {IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade});

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Patient>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<ReserveRecord>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Reservation>()
                .HasQueryFilter(u => !u.IsDelete);
        }
    }
}

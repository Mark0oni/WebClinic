using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Models;

namespace WebClinic.Data.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Service> Services { get; set; }   
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalCard> MedicalCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Doctor>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Service>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(d => d.Doctor)
                    .WithMany(d => d.Services)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Patient>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MedicalCard>(builder =>
            {
                builder.HasKey(p => p.Id);
                
                builder.ToTable("MedicalCards");

                builder.HasOne(d => d.Patient)
                .WithMany(d => d.MedicalCards)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

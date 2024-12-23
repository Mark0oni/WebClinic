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

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Service> Services { get; set; } 
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<MedicalCard> MedicalCards { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

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

            modelBuilder.Entity<Patient>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(d => d.User)
                       .WithMany()
                       .HasForeignKey(d => d.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(p => p.MedicalCard)
                       .WithOne(mc => mc.Patient)
                       .HasForeignKey<Patient>(p => p.MedicalCardId)
                       .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Service>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(d => d.Doctor)
                       .WithMany(d => d.Services)
                       .HasForeignKey(d => d.DoctorId)
                       .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Schedule>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasOne(s => s.Doctor)
                       .WithMany(d => d.Schedules)
                       .HasForeignKey(s => s.DoctorId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(s => s.Service)
                       .WithMany(sr => sr.Schedules)
                       .HasForeignKey(s => s.ServiceId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(s => new { s.DoctorId, s.StartTime, s.EndTime })
                       .IsUnique()
                       .HasDatabaseName("IX_Schedule_Doctor_Time");
            });

            modelBuilder.Entity<MedicalCard>(builder =>
            {
                builder.HasKey(mc => mc.Id);

                builder.HasOne(mc => mc.Patient)
                       .WithOne(p => p.MedicalCard)
                       .HasForeignKey<Patient>(p => p.MedicalCardId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Appointment>(builder =>
            {
                builder.HasKey(a => a.Id);

                builder.HasOne(a => a.Patient)
                       .WithMany(p => p.Appointments)
                       .HasForeignKey(a => a.PatientId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(a => a.Doctor)
                       .WithMany(d => d.Appointments)
                       .HasForeignKey(a => a.DoctorId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(a => a.Schedule)
                       .WithOne(s => s.Appointment)
                       .HasForeignKey<Appointment>(a => a.ScheduleId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}

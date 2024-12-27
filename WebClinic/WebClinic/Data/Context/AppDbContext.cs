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
        public DbSet<AppointmentResult> AppointmentResults { get; set; }
        public DbSet<Notification> Notifications { get; set; }

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

                builder.HasOne(s => s.Service)
                       .WithMany(sr => sr.Schedules)
                       .HasForeignKey(s => s.ServiceId)
                       .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<MedicalCard>(builder =>
            {
                builder.HasKey(mc => mc.Id);

                builder.HasOne(mc => mc.Patient)
                       .WithMany(p => p.MedicalCards)
                       .HasForeignKey(p => p.PatientId)
                       .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Appointment>(builder =>
            {
                builder.HasKey(a => a.Id);

                builder.HasOne(a => a.Patient)
                       .WithMany(p => p.Appointments)
                       .HasForeignKey(a => a.PatientId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(a => a.Schedule)
                       .WithMany(s => s.Appointments)
                       .HasForeignKey(a => a.ScheduleId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AppointmentResult>(builder =>
            {
                builder.HasKey(a => a.Id);

                builder.HasOne(a => a.Appointment)
                       .WithMany(p => p.AppointmentResults)
                       .HasForeignKey(a => a.AppointmentId)
                       .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Notification>(builder =>
            {
                builder.HasKey(a => a.Id);

                builder.HasOne(n => n.User)
                       .WithMany(u => u.Notifications)
                       .HasForeignKey(a => a.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

            });

            base.OnModelCreating(modelBuilder);
        }

    }
}

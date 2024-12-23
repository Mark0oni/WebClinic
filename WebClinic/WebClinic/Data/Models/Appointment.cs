namespace WebClinic.Data.Models
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }

    public class Appointment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public required DateTime AppointmentDate { get; set; } // Дата записи

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public string? Conclusion { get; set; } // Заключение врача
        
        public string? Prescription { get; set; } // Рецепт лекарств

        public required string PatientId { get; set; }

        public Patient Patient { get; set; }

        public required string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public required string ScheduleId { get; set; }

        public Schedule Schedule { get; set; }
    }
}

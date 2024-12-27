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
        public Guid Id { get; set; } = Guid.NewGuid();

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public required Guid PatientId { get; set; }

        public Patient? Patient { get; set; }

        public required Guid ScheduleId { get; set; }

        public Schedule? Schedule { get; set; }

        public List<AppointmentResult> AppointmentResults { get; set; } = [];
    }
}

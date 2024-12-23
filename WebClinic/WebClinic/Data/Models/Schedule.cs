namespace WebClinic.Data.Models
{
    public class Schedule
    {
        public string Id {  get; set; } = Guid.NewGuid().ToString();

        public required DateTime Date { get; set; }
        
        public required TimeSpan StartTime { get; set; }
        
        public required TimeSpan EndTime { get; set; }  

        public required bool IsAvailable { get; set; }

        public required string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public required string ServiceId { get; set; }

        public Service Service { get; set; }

        public string? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}

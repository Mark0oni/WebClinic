namespace WebClinic.Data.Models
{
    public class Schedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required DateTime Date { get; set; }
        
        public required TimeSpan StartTime { get; set; }
        
        public required TimeSpan EndTime { get; set; }

        public required bool IsAvailable { get; set; } = true;

        public required Guid ServiceId { get; set; }

        public Service? Service { get; set; }

        public List<Appointment> Appointments { get; set; } = [];
    }
}

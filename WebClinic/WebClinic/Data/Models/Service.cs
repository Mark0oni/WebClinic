namespace WebClinic.Data.Models
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string ServiceName { get; set; }

        public string? Description { get; set; }

        public required decimal Cost { get; set; }

        public required int Cabinet { get; set; }

        public Guid? DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public List<Schedule> Schedules { get; set; } = [];
    }
}

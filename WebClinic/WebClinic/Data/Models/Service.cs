namespace WebClinic.Data.Models
{
    public class Service
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string ServiceName { get; set; }

        public string? Description { get; set; }

        public required decimal Cost { get; set; }

        public required int Cabinet { get; set; }

        public string? DoctorId { get; set; }

        public Doctor? Doctor { get; set; }
    }
}

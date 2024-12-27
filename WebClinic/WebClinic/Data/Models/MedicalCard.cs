namespace WebClinic.Data.Models
{
    public class MedicalCard
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required Guid PatientId { get; set; }

        public Patient? Patient { get; set; }

        public List<Appointment> Appointments { get; set; } = [];
    }

    public class AppointmentResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Diagnosis { get; set; }

        public string? Prescription { get; set; }

        public required Guid AppointmentId { get; set; }
        
        public Appointment? Appointment { get; set; }
    }
}

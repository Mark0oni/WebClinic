namespace WebClinic.Data.Models
{
    public class MedicalCard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string PatientId { get; set; }

        public Patient Patient { get; set; }

        public List<MedicalRecord> Records { get; set; } = new();
    }

    public class MedicalRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required DateTime RecordDate { get; set; }

        public string? Diagnosis { get; set; }

        public string? Prescription { get; set; }

        public required string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public required string MedicalCardId { get; set; }
        
        public MedicalCard MedicalCard { get; set; }
    }
}

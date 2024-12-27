namespace WebClinic.Data.ViewModels.MedicalCard
{
    public class MedicalCardViewModel
    {
        
        public DateTime AppointmentDate { get; set; }
        
        public TimeSpan StartTime { get; set; }
        
        public string? Diagnosis { get; set; }
        
        public string? Prescription { get; set; }
        
        public string? DoctorFullName { get; set; }

        public string? PatientFullName { get; set; }

        public string? ServiceName { get; set; }

    }
}

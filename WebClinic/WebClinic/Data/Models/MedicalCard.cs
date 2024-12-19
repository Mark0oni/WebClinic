using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class MedicalCard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string PolicyNumber { get; set; }

        public required string IllnessHistory { get; set; }

        public required string PatientId { get; set; }

        public Patient? Patient { get; set; }
    }
}

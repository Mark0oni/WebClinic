using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Patient
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required DateTime DateOfBirth { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        public required string MedicalCardId { get; set; }

        public MedicalCard MedicalCard { get; set; }

        public List<Appointment> Appointments { get; set; } = []; 
    }
}

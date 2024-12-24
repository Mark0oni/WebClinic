using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Patient
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required DateTime DateOfBirth { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        public List<MedicalCard> MedicalCards { get; set; } = [];

        public List<Appointment> Appointments { get; set; } = []; 
    }
}

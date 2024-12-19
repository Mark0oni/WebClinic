using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Patient
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<MedicalCard> MedicalCards { get; set; } = [];

        public required string UserId { get; set; }

        public User? User { get; set; }
    }
}

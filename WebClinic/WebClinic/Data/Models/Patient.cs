using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Patient : Users
    {
        [Key]
        public int Id { get; set; }
        public ICollection<MedicalCard> MedicalCards { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }
}

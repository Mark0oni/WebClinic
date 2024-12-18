using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class MedicalCard
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Полис не может превышать 50 символов")]
        [DisplayName("Полис")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DisplayName("История болезней")]
        public string IllnessHistory { get; set; }

        [Required]
        public string PatientId { get; set; }

        [ForeignKey("UserId")]
        public Patient Patient { get; set; }
    }
}

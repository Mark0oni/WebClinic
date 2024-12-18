using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Doctor : Users
    {
        [Key]
        public string Id { get; set; }

        public string PostName { get; set; }

        public int Experience { get; set; }

        public decimal Salary { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }

}

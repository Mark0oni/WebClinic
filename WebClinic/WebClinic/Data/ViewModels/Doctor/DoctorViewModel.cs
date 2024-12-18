using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.ViewModels.Doctor
{
    public class DoctorViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string PostName { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Опыт должен быть от 0 до 50 лет")]
        [Display(Name = "Стаж")]
        public int Experience { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Зарплата")]
        public decimal Salary { get; set; }
    }
}

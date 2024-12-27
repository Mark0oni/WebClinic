using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.ViewModels.AdminDoctor
{
    public class DoctorViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [DisplayName("Фамилия")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [DisplayName("Имя")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        [DisplayName("Отчество")]
        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Должность")]
        public required string PostName { get; set; }


        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(0, 50, ErrorMessage = "Опыт должен быть от 0 до 50 лет")]
        [Display(Name = "Стаж")]
        public required int Experience { get; set; }


        [Required(ErrorMessage = "Это поле обязательно")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Зарплата")]
        public required decimal Salary { get; set; }

    }
}

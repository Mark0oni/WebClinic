using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.ViewModels.AdminDoctor
{
    public class EditDoctorViewModel
    {

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

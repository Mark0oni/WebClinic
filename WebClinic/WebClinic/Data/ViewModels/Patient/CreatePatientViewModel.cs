using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.Patient
{
    public class CreatePatientViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [DisplayName("Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Range(30, 200, ErrorMessage = "Вес должен быть в пределах от 30 до 200")]
        [DisplayName("Вес (кг)")]
        public int? Weight { get; set; }

        [Range(30, 250, ErrorMessage = "Рост должен быть в пределах от 30 до 250 см")]
        [DisplayName("Рост (см)")]
        public int? Height { get; set; }
    }
}

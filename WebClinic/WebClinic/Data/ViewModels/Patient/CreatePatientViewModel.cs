using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.ViewModels.Patient
{
    public class CreatePatientViewModel
    {

        [Required(ErrorMessage = "Это поле обязательно")]
        [DisplayName("Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Range(1, 500, ErrorMessage = "Вес должен быть в пределах от 1 до 200")]
        public int Weight { get; set; }

        [Range(30, 250, ErrorMessage = "Рост должен быть в пределах от 30 до 250 см")]
        public int Height { get; set; }

    }
}

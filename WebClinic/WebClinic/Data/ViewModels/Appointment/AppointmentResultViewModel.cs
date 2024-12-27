using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels.Appointment
{
    public class AppointmentResultViewModel
    {
        [StringLength(500, ErrorMessage = "Диагноз не может превышать 500 символов.")]
        [DisplayName("Диагноз")]
        public string? Diagnosis { get; set; }

        [StringLength(500, ErrorMessage = "Рецепт не может превышать 500 символов.")]
        [DisplayName("Рецепт")]
        public string? Prescription { get; set; }

        public required Guid AppointmentId { get; set; }
    }
}

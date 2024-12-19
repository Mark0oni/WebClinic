using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.Schedule
{
    public class CreateScheduleViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Date)]
        [DisplayName("Дата")]
        public required DateTime Date { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Time)]
        [DisplayName("Время начала приема")]
        public required TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Time)]
        [DisplayName("Время окончания приема")]
        public required TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DisplayName("Доступность")]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Услуга")]
        public required string ServiceId { get; set; }
    }
}

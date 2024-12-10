using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}

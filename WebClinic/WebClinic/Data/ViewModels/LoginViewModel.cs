using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Password, ErrorMessage = "Введите корректный пароль")]
        [Display(Name = "Пароль")]
        public string Passsword { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }

    }
}

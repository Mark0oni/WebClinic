using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}

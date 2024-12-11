using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Password, ErrorMessage = "Введите корректный пароль")]
        public string Passsword { get; set; }

        public bool RememberMe { get; set; }

    }
}

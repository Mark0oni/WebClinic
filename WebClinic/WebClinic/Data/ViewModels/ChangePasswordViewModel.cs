using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [DataType(DataType.Password, ErrorMessage = "Пароль должен содержать хотя бы одну цифру")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Пароль не совпадает")]
        [DisplayName("Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [DataType(DataType.Password, ErrorMessage = "Пароль должен содержать хотя бы одну цифру")]
        [DisplayName("Повторить новый пароль")]
        public string ConfirmNewPassword { get; set; }
    }
}

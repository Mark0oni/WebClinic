using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        [Compare("ConfirNewPassword", ErrorMessage = "Пароль не совпадает")]
        [Display(Name = "Новый Пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторить новый пароль")]
        public string ConfirNewPassword { get; set; }
    }
}

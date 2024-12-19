using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [DisplayName("Фамилия")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [DisplayName("Имя")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        [DisplayName("Отчество")]
        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [DataType(DataType.Password, ErrorMessage = "Пароль должен содержать хотя бы одну цифру")]
        [DisplayName("Пароль")]
        public required string Password { get; set; }

    }
}

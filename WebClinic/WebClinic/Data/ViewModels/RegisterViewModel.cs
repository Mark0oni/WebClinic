using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels
{
    public class RegisterViewModel
    {
        public enum Genders
        {
            Мужской,
            Женский
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        public string MiddleName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        public Genders Gender { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Пароль не совпадает")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

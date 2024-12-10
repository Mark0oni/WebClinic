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
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Пол")]
        public Genders Gender { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Пароль не совпадает")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторить пароль")]
        public string ConfirmPassword { get; set; }
    }
}

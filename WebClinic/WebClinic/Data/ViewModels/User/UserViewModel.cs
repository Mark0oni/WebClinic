using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.User
{
    public class UserViewModel
    {
        public required string Id { get; set; }

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
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public required string Roles { get; set; }

    }
}

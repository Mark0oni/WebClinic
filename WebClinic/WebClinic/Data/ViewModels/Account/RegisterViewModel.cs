﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Отчество не может превышать 50 символов")]
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [DataType(DataType.Password, ErrorMessage = "Пароль должен содержать хотя бы одну цифру")]
        [Compare("ConfirmPassword", ErrorMessage = "Пароль не совпадает")]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [DataType(DataType.Password, ErrorMessage = "Пароль должен содержать хотя бы одну цифру")]
        [DisplayName("Повторить пароль")]
        public string ConfirmPassword { get; set; }
    }
}

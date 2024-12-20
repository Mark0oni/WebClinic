﻿using System.ComponentModel.DataAnnotations;

namespace WebClinic.Data.ViewModels.Account
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClinic.Data.ViewModels.Service
{
    public class EditServiceViewModel
    {

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Название услуги не может превышать 50 символов")]
        [DisplayName("Услуга")]
        public required string ServiceName { get; set; }

        [StringLength(250, ErrorMessage = "Описание не может превышать 250 символов")]
        [DisplayName("Описание")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(0, 101, ErrorMessage = "Номер кабинета должен быть от 0 до 101")]
        [DisplayName("Кабинет")]
        public required int Cabinet { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Стоимость")]
        public required decimal Cost { get; set; }

        [Display(Name = "Врач")]
        public Guid? DoctorId { get; set; }
    }
}

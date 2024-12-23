﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Doctor
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string PostName { get; set; }

        public required int Experience { get; set; }

        public required decimal Salary { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        public List<Service> Services { get; set; } = [];

        public List<Schedule> Schedules { get; set; } = [];
    }

}

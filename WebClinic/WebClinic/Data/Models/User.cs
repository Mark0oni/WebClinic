using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace WebClinic.Data.Models
{
    public class User : IdentityUser
    {
        public required string LastName { get; set; }

        public required string FirstName { get; set; }

        public required string MiddleName { get; set; }

        public List<IdentityRole> Roles { get; set; } = [];

        public List<Notification> Notifications { get; set; } = [];
    }
}

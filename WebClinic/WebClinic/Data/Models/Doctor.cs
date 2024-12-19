using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Data.Models
{
    public class Doctor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string PostName { get; set; }

        public required int Experience { get; set; }

        public required decimal Salary { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        public List<Service> Services { get; set; } = [];
    }

}

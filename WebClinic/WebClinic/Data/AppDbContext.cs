using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Models;

namespace WebClinic.Data;
public class AppDbContext : IdentityDbContext<User>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
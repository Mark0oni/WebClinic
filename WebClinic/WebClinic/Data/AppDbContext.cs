using WebClinic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebClinic.Data;
public class AppDbContext : DbContext

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
}
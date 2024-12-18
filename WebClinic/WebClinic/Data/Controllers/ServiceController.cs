using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.Service;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Админ,Доктор")]
    [Route("[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.Doctor)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    Cabinet = s.Cabinet,
                    Cost = s.Cost,
                    DoctorName = s.Doctor != null ? s.Doctor.User!.UserName : "Не назначен"
                })
                .ToListAsync();

            return View(services);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

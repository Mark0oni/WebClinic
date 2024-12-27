using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.DoctorService;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Доктор")]
    [Route("[controller]")]
    [ApiController]
    public class DoctorServiceController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorServiceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден!";
                return NotFound();
            }

            var services = await _context.Services
                .Where(s => s.DoctorId == currentDoctor.Id)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    Cabinet = s.Cabinet,
                    Cost = s.Cost
                })
                .ToListAsync();

            return View(services);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateServiceViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var service = new Service
                {
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                    Cabinet = model.Cabinet,
                    Cost = model.Cost,
                    DoctorId = currentDoctor.Id
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден!";
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && s.DoctorId == currentDoctor.Id);

            if (service == null)
            {
                TempData["ErrorMessage"] = "Услуга не найдена или вы не имеете доступа.";
                return Forbid();
            }

            var model = new EditServiceViewModel
            {
                ServiceName = service.ServiceName,
                Description = service.Description,
                Cabinet = service.Cabinet,
                Cost = service.Cost
            };

            return View(model);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] EditServiceViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден!";
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && s.DoctorId == currentDoctor.Id);

            if (service == null)
            {
                TempData["ErrorMessage"] = "Услуга не найдена или вы не имеете доступа.";
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                service.ServiceName = model.ServiceName;
                service.Description = model.Description;
                service.Cabinet = model.Cabinet;
                service.Cost = model.Cost;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден!";
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Schedules) 
                .FirstOrDefaultAsync(s => s.Id == id && s.DoctorId == currentDoctor.Id);

            if (service == null)
            {
                TempData["ErrorMessage"] = "Услуга не найдена или вы не имеете доступа.";
                return Forbid();
            }

            if (service.Schedules != null && service.Schedules.Any())
            {
                TempData["ErrorMessage"] = "Невозможно удалить услугу, так как на нее уже записаны пациенты.";
                return RedirectToAction(nameof(Index));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Услуга успешно удалена.";
            return RedirectToAction(nameof(Index));
        }

    }
}

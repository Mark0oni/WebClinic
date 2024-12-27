using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.MedicalCard;

namespace WebClinic.Controllers
{
    [Authorize (Roles = "Пациент")]
    [ApiController]
    [Route("[controller]")]
    public class MedicalCardController : Controller
    {
        private readonly AppDbContext _context;

        public MedicalCardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "Пациент не найден.";
                return RedirectToAction("Index", "Home");
            }

            var records = await _context.MedicalCards
                .Where(mc => mc.PatientId == patient.Id)
                .Include(mc => mc.Appointments)
                    .ThenInclude(a => a.Schedule)
                        .ThenInclude(s => s.Service)
                            .ThenInclude(s => s.Doctor)
                .SelectMany(mc => mc.Appointments)
                .Select(a => new MedicalCardViewModel
                {
                    AppointmentDate = a.Schedule.Date,
                    StartTime = a.Schedule.StartTime,
                    Diagnosis = a.AppointmentResults.FirstOrDefault().Diagnosis ?? "Нет диагноза",
                    Prescription = a.AppointmentResults.FirstOrDefault().Prescription ?? "Нет рецепта",
                    ServiceName = a.Schedule.Service.ServiceName,
                    DoctorFullName = a.Schedule.Service != null && a.Schedule.Service.Doctor != null
                       ? $"{a.Schedule.Service.Doctor.User.LastName} {a.Schedule.Service.Doctor.User.FirstName} {a.Schedule.Service.Doctor.User.MiddleName}"
                       : "Неизвестный врач",
                })
                .ToListAsync();

            if (!records.Any())
            {
                TempData["ErrorMessage"] = "Нет записей в медицинской карте.";
                return RedirectToAction("Index", "Home");
            }

            return View(records);
        }
    }
}

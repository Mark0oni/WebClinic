using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebClinic.Data.ViewModels.Appointment;

namespace WebClinic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppointmentController(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Пациент")]
        [HttpGet("patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patient.Id)
                .Include(a => a.Schedule)
                .ThenInclude(s => s.Service)
                .ThenInclude(s => s.Doctor)
                .ThenInclude(s => s.User)
                .ToListAsync();

            return View(appointments);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetMyPatients(bool showCompleted = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Доктор не найден.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShowCompleted"] = showCompleted;

            var query = _context.Appointments
                .Include(a => a.Schedule)
                .ThenInclude(s => s.Service)
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Where(a => a.Schedule.Service.DoctorId == doctor.Id);

            if (!showCompleted)
            {
                query = query.Where(a => a.Status == AppointmentStatus.Scheduled);
            }

            var appointments = await query.ToListAsync();

            return View(appointments);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("addAppointmentResult/{id:guid}")]
        public async Task<IActionResult> AddAppointmentResult(Guid id)
        {

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null || appointment.Status != AppointmentStatus.Scheduled)
            {
                TempData["ErrorMessage"] = " Запись на прием не найдена или уже завершена.";
                return RedirectToAction(nameof(Index));
            }

            var model = new AppointmentResultViewModel
            {
                AppointmentId = appointment.Id,
            };

            return View(model);
        }

        [Authorize(Roles = "Доктор")]
        [HttpPost("addAppointmentResult/{id:guid}")]
        public async Task<IActionResult> AddAppointmentResult([FromForm] AppointmentResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == model.AppointmentId);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Запись на прием не найдена.";
                    return RedirectToAction(nameof(Index));
                }

                var appointmentResult = new AppointmentResult
                {
                    Diagnosis = model.Diagnosis,
                    Prescription = model.Prescription,
                    AppointmentId = appointment.Id
                };

                appointment.AppointmentResults.Add(appointmentResult);

                appointment.Status = AppointmentStatus.Completed;

                _context.AppointmentResults.Add(appointmentResult);
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction("GetMyPatients");
                
            }

            return View(model);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("cancelAppointment/{id:guid}")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Schedule)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Запись на прием не найдена.";
                return RedirectToAction(nameof(Index));
            }

            if (appointment.Status != AppointmentStatus.Scheduled)
            {
                TempData["ErrorMessage"] = "Нельзя отменить запись, которая уже завершена или отменена.";
                return RedirectToAction(nameof(Index));
            }

            appointment.Status = AppointmentStatus.Cancelled;

            if (appointment.Schedule != null)
            {
                appointment.Schedule.IsAvailable = true;
                _context.Schedules.Update(appointment.Schedule);
            }

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetMyPatients");
        }

    }
}

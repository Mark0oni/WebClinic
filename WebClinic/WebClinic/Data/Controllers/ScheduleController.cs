using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.Schedule;

namespace WebClinic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Доктор")]
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

            var schedules = await _context.Schedules
                .Include(s => s.Service)
                .ThenInclude(s => s.Doctor)
                .Select(s => new ScheduleViewModel
                {
                    Id = s.Id,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsAvailable = s.IsAvailable,
                    ServiceName = s.Service != null 
                        ? s.Service.ServiceName 
                        : "Не назначена" 
                })
                .ToListAsync();


            return View(schedules);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
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
                .ToListAsync();

            if (services.Count == 0)
            {
                TempData["ErrorMessage"] = "У вас нет услуг, добавьте услугу, чтобы создать расписание.";
            }

            ViewBag.Services = new SelectList(services, "Id", "ServiceName");
            return View();
        }

        [Authorize(Roles = "Доктор")]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EndTime <= model.StartTime)
                {
                    TempData["ErrorMessage"] = "Время окончания не может быть раньше или совпадать с временем начала.";
                    
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
                        .ToListAsync();

                    if (services.Count == 0)
                    {
                        TempData["ErrorMessage"] = "У вас нет услуг, добавьте услугу, чтобы создать расписание.";
                    }

                    ViewBag.Services = new SelectList(services, "Id", "ServiceName");
                    return View(model);
                } else
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
                        .ToListAsync();

                    if (services.Count == 0)
                    {
                        TempData["ErrorMessage"] = "У вас нет услуг, добавьте услугу, чтобы создать расписание.";
                        
                        return RedirectToAction("Create", "Service");
                    }

                    var schedule = new Schedule
                    {
                        Date = model.Date,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        IsAvailable = model.IsAvailable,
                        ServiceId = model.ServiceId
                    };

                    _context.Schedules.Add(schedule);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            } 

            return View(model);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Service)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            var model = new EditScheduleViewModel
            {
                Date = schedule.Date,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                IsAvailable = schedule.IsAvailable,
                ServiceId = schedule.ServiceId,
                DoctorId = schedule.Service!.DoctorId,
            };

            var services = await _context.Services
                .Where(s => s.DoctorId == schedule.Service!.DoctorId)
                .ToListAsync();

            ViewBag.Services = new SelectList(services, "Id", "ServiceName", model.ServiceId);

            return View(model);
        }

        [Authorize(Roles = "Доктор")]
        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] EditScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EndTime <= model.StartTime)
                {
                    TempData["ErrorMessage"] = "Время окончания не может быть раньше или совпадать с временем начала.";

                    var services = await _context.Services
                        .Where(s => s.DoctorId == model.DoctorId)
                        .ToListAsync();

                    ViewBag.Services = new SelectList(services, "Id", "ServiceName", model.ServiceId);

                    return View(model);
                }

                var schedule = await _context.Schedules
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (schedule == null)
                {
                    return NotFound();
                }

                schedule.Date = model.Date;
                schedule.StartTime = model.StartTime;
                schedule.EndTime = model.EndTime;
                schedule.IsAvailable = model.IsAvailable;
                schedule.ServiceId = model.ServiceId;

                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var servicesForDoctor = await _context.Services
                .Where(s => s.DoctorId == model.DoctorId)
                .ToListAsync();

            ViewBag.Services = new SelectList(servicesForDoctor, "Id", "ServiceName", model.ServiceId);

            return View(model);
        }

        [Authorize(Roles = "Доктор")]
        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var schedule = await _context.Schedules
                .Include(a => a.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            if (schedule.Appointments.Any()) 
            {
                TempData["ErrorMessage"] = "Невозможно удалить время приема, потому что уже есть запись.";
                return RedirectToAction(nameof(Index));
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Пациент")]
        [HttpGet("getAvailableDates")]
        public async Task<IActionResult> GetAvailableDates()
        {
            var schedules = await _context.Schedules
               .Include(s => s.Service)
               .ThenInclude(s => s.Doctor)
               .Where(s => s.IsAvailable)
               .Select(s => new AvailableDatesScheduleViewModel
               {
                   Id = s.Id,
                   Date = s.Date,
                   StartTime = s.StartTime,
                   EndTime = s.EndTime,
                   Cabinet = s.Service.Cabinet,
                   Description = s.Service.Description,
                   ServiceName = s.Service != null
                       ? s.Service.ServiceName
                       : "Не назначена",
                   DoctorFullName = s.Service != null && s.Service.Doctor != null
                       ? $"{s.Service.Doctor.User.LastName} {s.Service.Doctor.User.FirstName} {s.Service.Doctor.User.MiddleName}"
                       : "Неизвестный врач",
                   PostName = s.Service != null && s.Service.Doctor != null
                        ? $"{s.Service.Doctor.PostName}"
                        : "Неизвестная должность",
                   Experience = s.Service != null && s.Service.Doctor != null
                        ? $"{s.Service.Doctor.Experience}"
                        : "Неизвестный опыт"
               })
               .ToListAsync();


            return View(schedules);
        }

        [Authorize(Roles = "Пациент")]
        [HttpPost("createAppointment")]
        public async Task<IActionResult> CreateAppointment([FromForm] Guid scheduleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "Пациент не найден.";
                return RedirectToAction(nameof(Index));
            }

            var schedule = _context.Schedules.FirstOrDefault(s => s.Id == scheduleId && s.IsAvailable);

            if (schedule == null)
            {
                TempData["ErrorMessage"] = "Расписание недоступно.";
                return RedirectToAction(nameof(Index));
            }

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.Scheduled
            };

            schedule.IsAvailable = false;

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return RedirectToAction("GetAvailableDates", "Schedule");
        }
    }
}

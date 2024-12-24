using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.Schedule;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebClinic.Data.Context;
using System.Security.Claims;
using System.Numerics;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Доктор")]
    [Route("[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Service)
                .ThenInclude(s => s.Doctor)
                .Select(s => new ScheduleViewModel
                {
                    Id = s.Id.ToString(),
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

        [HttpGet("edit/{id}")]
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

        [HttpPost("edit/{id}")]
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


        [HttpGet("delete/{id}")]
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
                TempData["ErrorMessage"] = "Нельзя удалить слот расписания, потому что уже есть запись.";
                return BadRequest();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

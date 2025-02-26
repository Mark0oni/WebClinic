﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.AdminService;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Админ")]
    [Route("[controller]")]
    [ApiController]
    public class AdminServiceController : Controller
    {
        private readonly AppDbContext _context;

        public AdminServiceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(u => u.Doctor)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    Cabinet = s.Cabinet,
                    Cost = s.Cost,
                    DoctorName = s.Doctor != null 
                        ? $"{s.Doctor.User.LastName} {s.Doctor.User.FirstName} {s.Doctor.User.MiddleName}" 
                        : "Не назначен"
                })
                .ToListAsync();

            return View(services);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = $"{d.User.LastName} {d.User.FirstName} {d.User.MiddleName}"
                })
                .ToListAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentDoctor != null)
            {
                ViewBag.DoctorId = currentDoctor.Id;
            }
            else
            {
                ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");
            }

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateServiceViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

            if (ModelState.IsValid)
            {

                if (currentDoctor != null)
                {
                    model.DoctorId = currentDoctor.Id;
                }

                var service = new Service
                {
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                    Cabinet = model.Cabinet,
                    Cost = model.Cost,
                    DoctorId = model.DoctorId
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Услуга успешно добавлена.";
                return RedirectToAction(nameof(Index));
            }

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = $"{d.User.LastName} {d.User.FirstName} {d.User.MiddleName}"
                })
                .ToListAsync();

            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var service = await _context.Services
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            var model = new EditServiceViewModel
            {
                ServiceName = service.ServiceName,
                Description = service.Description,
                Cabinet = service.Cabinet,
                Cost = service.Cost,
                DoctorId = service.DoctorId
            };

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = $"{d.User.LastName} {d.User.FirstName} {d.User.MiddleName}"
                })
                .ToListAsync();

            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName", model.DoctorId);

            return View(model);
        }


        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] EditServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _context.Services
                    .Include(s => s.Doctor)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (service == null)
                {
                    return NotFound();
                }

                service.ServiceName = model.ServiceName;
                service.Description = model.Description;
                service.Cabinet = model.Cabinet;
                service.Cost = model.Cost;
                service.DoctorId = model.DoctorId;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = $"{d.User.LastName} {d.User.FirstName} {d.User.MiddleName}"
                })
                .ToListAsync();

            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName", model.DoctorId);

            return View(model);
        }


        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Услуга успешно удалена.";
            return RedirectToAction(nameof(Index));
        }
    }
}

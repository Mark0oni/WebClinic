﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.Doctor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Админ")]
    [Route("admin/[controller]")]
    [ApiController]
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DoctorController(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User) 
                .Select(d => new DoctorViewModel
                {
                    Id = d.Id.ToString(),
                    LastName = d.User.LastName,
                    FirstName = d.User.FirstName,
                    MiddleName = d.User.MiddleName,
                    Email = d.User.Email,
                    PostName = d.PostName,
                    Experience = d.Experience,
                    Salary = d.Salary
                })
                .ToListAsync();

            return View(doctors);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);


                if (user == null)
                {
                    TempData["ErrorMessage"] = "Пользователь с таким email не найден!";
                    return View(model);
                }

                if (user.LastName != model.LastName || user.FirstName != model.FirstName || user.MiddleName != model.MiddleName)
                {
                    TempData["ErrorMessage"] = "ФИО не совпадает с данными пользователя!";
                    return View(model);
                }

                if (await _context.Doctors.AnyAsync(d => d.UserId == user.Id))
                {
                    TempData["ErrorMessage"] = "Этот пользователь уже зарегистрирован как врач!";
                    return View(model);
                }

                if (await _userManager.IsInRoleAsync(user, "Пациент"))
                {
                    TempData["ErrorMessage"] = "Пользователь с ролью 'Пациент' не может быть зарегистрирован как врач!";
                    return View(model);
                }

                var doctor = new Doctor
                {
                    PostName = model.PostName,
                    Experience = model.Experience,
                    Salary = model.Salary,
                    UserId = user.Id
                };

                _context.Doctors.Add(doctor);

                if (await _userManager.IsInRoleAsync(user, "Гость"))
                {
                    var removeGuestResult = await _userManager.RemoveFromRoleAsync(user, "Гость");
                    if (!removeGuestResult.Succeeded)
                    {
                        TempData["ErrorMessage"] = "Не удалось удалить роль 'Гость'.";
                        return View(model);
                    }
                }

                await _userManager.AddToRoleAsync(user, "Доктор");

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            var model = new EditDoctorViewModel
            {
                PostName = doctor.PostName,
                Experience = doctor.Experience,
                Salary = doctor.Salary
            };

            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] EditDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                {
                    return NotFound();
                }

                doctor.PostName = model.PostName;
                doctor.Experience = model.Experience;
                doctor.Salary = model.Salary;

                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            await _context.Users.Where(d => d.Id == doctor.UserId).ExecuteDeleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels.Patient;

namespace WebClinic.Controllers
{
    [Authorize(Roles="Гость")]
    [Route("[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PatientController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreatePatientViewModel model)
        {
            if (ModelState.IsValid == false) 
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentPatient = await _context.Patients
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (currentPatient != null)
            {
                TempData["ErrorMessage"] = "Пациент уже зарегестрирован!";
                return RedirectToAction(nameof(Index));
            }

            var patient = new Patient
            {
                DateOfBirth = model.DateOfBirth,
                UserId = userId,
                Weight = model.Weight,
                Height = model.Height
            };

            _context.Add(patient);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstAsync(d => d.Id == userId);

            var removeGuestResult = await _userManager.RemoveFromRoleAsync(user, "Гость");
            if (!removeGuestResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Не удалось удалить роль 'Гость'.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.AddToRoleAsync(user, "Пациент");

            await _signInManager.RefreshSignInAsync(user);

            TempData["Message"] = "Вы успешно оформили профиль пациента";
            return RedirectToAction("Index", "Home");
        }
    }
}

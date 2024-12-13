using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebClinic.Data.Models;
using WebClinic.Data.ViewModels;

namespace WebClinic.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.Select(user => new {
                user.Id,
                user.UserName,
                user.Email,
                user.LastName,
                user.FirstName,
                user.MiddleName
            }).ToList();

            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.LastName,
                user.FirstName,
                user.MiddleName
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new Users
            {
                UserName = model.Email,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            // Назначить роль по умолчанию (например, "Patient")
            await _userManager.AddToRoleAsync(user, "Patient");

            return Ok(new { Message = "Пользователь успешно создан", UserId = user.Id });
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] RegisterViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "Пользователь успешно обновлен" });
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "Пользователь успешно удален" });
        }

        // GET: api/Users/{id}/Roles
        [HttpGet("{id}/Roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        // POST: api/Users/{id}/Roles
        [HttpPost("{id}/Roles")]
        public async Task<IActionResult> AddRoleToUser(string id, [FromBody] string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            if (!await _roleManager.RoleExistsAsync(role))
                return BadRequest(new { Message = "Роль не существует" });

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "Роль успешно добавлена пользователю" });
        }

        // DELETE: api/Users/{id}/Roles
        [HttpDelete("{id}/Roles")]
        public async Task<IActionResult> RemoveRoleFromUser(string id, [FromBody] string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден" });

            if (!await _roleManager.RoleExistsAsync(role))
                return BadRequest(new { Message = "Роль не существует" });

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "Роль успешно удалена у пользователя" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebClinic.Data.Controllers
{

    [ApiController]
    [Route("api/")]
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [Route("Account/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Account/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}

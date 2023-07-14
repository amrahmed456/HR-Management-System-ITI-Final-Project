using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers.Auth
{
    public class AuthController : Controller
    {

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

    }
}

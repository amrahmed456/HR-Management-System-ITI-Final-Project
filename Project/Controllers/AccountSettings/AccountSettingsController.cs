using FinalProject.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers.AccountSettings
{
    [Authorize]
    public class AccountSettingsController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}

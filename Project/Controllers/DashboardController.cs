using FinalProject.Repository.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IEmployeeAttendanceRepository _attendanceRepo;
        private IEmployeeRepository _emloyeeRepo;
        private IDepartmentRepository _departmentRepo;

        public DashboardController(
            IEmployeeAttendanceRepository attendanceRepo,
            IEmployeeRepository emloyeeRepo,
            IDepartmentRepository departmentRepo
        )
        {
            _emloyeeRepo = emloyeeRepo;
            _attendanceRepo = attendanceRepo;
            _departmentRepo = departmentRepo;

        }
        public IActionResult Index()
        {
            ViewData["employeesCount"] = _emloyeeRepo.Count();
            ViewData["departmentsCount"] = _departmentRepo.Count();
            ViewData["usersCount"] = _emloyeeRepo.getUsersCount();
            var attendanceOfToday = _attendanceRepo.GetAttendanceForToday();
			ViewData["departmentsList"] = _departmentRepo.GetAll();
			return View(attendanceOfToday);
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Login", "Account", new { returnUrl = HttpContext.Request.Path });
        }
    }
}

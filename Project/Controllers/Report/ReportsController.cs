using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;
using FinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers.Report
{
    [Authorize]
    public class ReportsController : Controller
    {

        private IAttendanceReportRepository _reportRepo;

        public ReportsController
        (
            IAttendanceReportRepository reportRepo
        )
        {
            _reportRepo = reportRepo;
        }
      

        public IActionResult Index([FromQuery(Name = "data-range")] string dateRange)
        {
			DateTime? startDate = null, endDate = null;
			if (!string.IsNullOrEmpty(dateRange))
			{
				var dateParts = dateRange.Split(" - ");
				if (dateParts.Length == 2)
				{
					DateTime.TryParse(dateParts[0], out var parsedStartDate);
					DateTime.TryParse(dateParts[1], out var parsedEndDate);
					startDate = parsedStartDate;
					endDate = parsedEndDate;
					if (endDate.HasValue)
					{
						endDate = endDate.Value.Date.AddDays(1).AddSeconds(-1);
					}
				}
			}

			List<AttendanceReport> reports = _reportRepo.GetAll(startDate, endDate);
            return View(reports);
        }


        public IActionResult Print([FromRoute] int id)
        {
            var report = _reportRepo.GetById(id);
            if (report == null)
            {
                return RedirectToAction("Index");
            }
            return View(report);
        }


    }
}






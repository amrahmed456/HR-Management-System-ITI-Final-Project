using Azure.Core.GeoJson;
using FinalProject.Enums;
using FinalProject.Migrations;
using FinalProject.Models;
using FinalProject.Repository;
using FinalProject.Repository.interfaces;
using FinalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Constants;
using System.Security.Permissions;

namespace FinalProject.Controllers.Attendance
{
    [Authorize]
    public class AttendanceController : Controller
    {

        private IEmployeeAttendanceRepository _attendanceRepo;
        private IEmployeeRepository _employeeRepo;
        private IGeneralSettingsRepository _gsRepo;
        private IVacationRepository _vacationRepo;
        private IAttendanceReportRepository _reportRepo;

        public AttendanceController(
            IEmployeeAttendanceRepository attendanceRepo,
            IEmployeeRepository employeeRepo,
            IGeneralSettingsRepository gs,
            IVacationRepository vacationRepo,
            IAttendanceReportRepository reportRepo
        )
        {
            _attendanceRepo = attendanceRepo;
            _employeeRepo = employeeRepo;
            _gsRepo = gs;
            _vacationRepo = vacationRepo;
            _reportRepo = reportRepo;
        }

        [Authorize(Permissions.EmployeeAttendance.View)]
        public IActionResult Index([FromQuery(Name = "data-range")] string dateRange)
        {
            string responseData = TempData["ResponseData"] as string;
            if (!string.IsNullOrEmpty(responseData))
            {
                ViewData["Response"] = responseData;
            }

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
            ViewData["dataRange"] = dateRange;
            var EmpsAttendance = _attendanceRepo.GetAll(startDate, endDate);
            return View(EmpsAttendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.EmployeeAttendance.Create)]
        public IActionResult Create(AttendanceFormViewModel attendance)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (InsertNewAttendance(attendance))
                    {
                        TempData["ResponseData"] = Responses.success.ToString();
                        return RedirectToAction("Index");
                    }
                    
                }
                catch (Exception e)
                {
                    TempData["ResponseData"] = e.Message;
                    return RedirectToAction("Index");
                }
            }
            TempData["ResponseData"] = "Check out time must be greater than Check in time";
            return RedirectToAction("Index");
        }

        [Authorize(Permissions.EmployeeAttendance.Create)]
        private bool InsertNewAttendance(AttendanceFormViewModel attendance)
        {
           
            var employee = _employeeRepo.GetBySsn(attendance.Ssn);
            if (employee != null)
            {
                // check if employee already took the attendance for today

                var gs = _gsRepo.GetById(1);

                AttendanceHelper.officialHoliday = _vacationRepo.GetVacationOfToday();
                AttendanceHelper.Report = _reportRepo.GetEmployeeAttendanceReportForThisMonthOfTheYear(employee.Id);
                var EmplAttendance = AttendanceHelper.CalculateAllRequiredParameters(attendance, gs, employee);


                _attendanceRepo.Insert(EmplAttendance);
                return true;
            }
            
           
            return false;
        }

        [HttpPost]
        public IActionResult getEmployees()
        {
            var Emps = new List<EmployeeAttendanceFormModel>();
            bool isVacation = false;
            string todayName = DateTime.Today.DayOfWeek.ToString().ToLower();
            var gs = _gsRepo.GetById(1);
            if (gs.weekly_vacation1 == todayName || gs.weekly_vacation2 == todayName)
            {
                isVacation = true;
            }
            if (!isVacation)
            {
                var vacation = _vacationRepo.GetVacationOfToday();
                if (vacation != null)
                {
                    isVacation = true;
                }
            }
           
            if (!isVacation)
            {
                var employees = _attendanceRepo.GetEmployeesThatDidntAttendForToday();
                foreach (var emp in employees)
                {
                    var newEmp = new EmployeeAttendanceFormModel()
                    {
                        Ssn = emp.Ssn,
                        Name = emp.Name,
                        DepartmentName = emp.Department.Name
                    };
                    Emps.Add(newEmp);
                }
            }
            
            return Json(Emps);
        }

        [HttpGet]
        [Authorize(Permissions.EmployeeAttendance.Edit)]
        public IActionResult Edit(int id)
        {
            if (id != null || id != 0)
            {
                var attendance = _attendanceRepo.GetById(id);
                if (attendance != null)
                {
                    return View(attendance);
                }
                
            }
            TempData["ResponseData"] = "Invalid Attendance Model";
            return RedirectToAction("Index");
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.EmployeeAttendance.Edit)]
        public IActionResult Edit(AttendanceFormViewModel attendance,[FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var editModel = _attendanceRepo.GetById(id);
                if (editModel != null)
                {
                    try
                    {
                        var gs = _gsRepo.GetById(1);
                        AttendanceHelper.officialHoliday = _vacationRepo.GetVacationOfToday();
                        AttendanceHelper.EditModel = editModel;
                        AttendanceHelper.Report = _reportRepo.GetEmployeeAttendanceReportForThisMonthOfTheYear(editModel.EmployeeId);
                        var updatedModel = AttendanceHelper.CalculateAllRequiredParameters(attendance, gs, editModel.Employee, true);
                        
                        _attendanceRepo.Update(updatedModel , editModel);
                        TempData["ResponseData"] = Responses.success.ToString();
                        return RedirectToAction("Index");
                    }
                    catch(Exception e)
                    {
                        TempData["ResponseData"] = "Error: " + e.Message;
                        return RedirectToAction("Edit", id);
                    }
                   
                }
                  
            }

            TempData["ResponseData"] = "Invalid Attendance Model";
            return RedirectToAction("Index");
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.EmployeeAttendance.Delete)]
        public IActionResult Delete(int id)
        {
            if (id != null || id != 0)
            {
                try
                {
                    var attendance = _attendanceRepo.GetById(id);
                    if (attendance != null)
                    {
                        _attendanceRepo.Delete(attendance);
                        TempData["ResponseData"] = Responses.success.ToString();
                        return RedirectToAction("Index");
                    }
                }catch(Exception e)
                {
                    TempData["ResponseData"] = "Error: " + e.Message;
                    return RedirectToAction("Index");
                }
            }
            TempData["ResponseData"] = "Invalid Attendance Model";
            return RedirectToAction("Index");
        }

        [Authorize(Permissions.EmployeeAttendance.Create)]
        public IActionResult UploadExcel()
        {
            var employees = _employeeRepo.GetAll();
            return View(employees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.EmployeeAttendance.Create)]
        public ActionResult ImportFromExcel(IFormFile file)
        {
            string errors = "";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    int ssn;
                    if (int.TryParse(worksheet.Cells[row, 1].Value.ToString(), out ssn))
                    {
                        var checkIn = DateTime.Parse(worksheet.Cells[row, 2].Value.ToString());
                        var checkOut = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString());
                        if (!_attendanceRepo.IsAttendanceTakenForToday(ssn))
                        {
                            var attendance = new AttendanceFormViewModel()
                            {
                                Ssn = ssn,
                                CheckIn = checkIn,
                                CheckOut = checkOut
                            };
                            if (!InsertNewAttendance(attendance))
                            {
                                errors += " There was an error while saving some employee's attendance, it maybe due to check in is greater than check out time for some employee  ";
                            }
                        }
                        else
                        {
                            errors += " Some users already took the attendance for today so we skipped it ";
                        }
                    }
                    else
                    {
                        errors += " Some Ssn not written as numbers and skipped ";
                    }

                    
                }
            }

            if (errors != "")
            {
                TempData["ResponseData"] = errors;
            }
            else
            {
                TempData["ResponseData"] = Responses.success.ToString();
            }
           
            return RedirectToAction("Index");
        }

        public IActionResult exportpdf([FromQuery(Name = "data-range")] string dateRange)
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
            var data = _attendanceRepo.GetAll(startDate, endDate);

            // Create a new PDF document
            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Add the data to the PDF document
            var table = new PdfPTable(5);
            table.AddCell("Employee");
            table.AddCell("Department");
            table.AddCell("Check in");
            table.AddCell("Check Out");
            table.AddCell("Date");
            foreach (var item in data)
            {
                table.AddCell(item.Employee.Name);
                table.AddCell(item.Department);
                table.AddCell(item.CheckIn.ToString("hh\\:mm\\ tt"));
                table.AddCell(item.CheckOut.ToString("hh\\:mm\\ tt"));
                table.AddCell(item.CreatedAt.ToShortDateString());
            }
            document.Add(table);

            // Close the PDF document
            document.Close();
            writer.Close();
            memoryStream.Close();

            // Return the PDF file as a FileStreamResult
            var pdfBytes = memoryStream.ToArray();
            var fileStream = new MemoryStream(pdfBytes);
            return new FileStreamResult(fileStream, "application/pdf")
            {
                FileDownloadName = "Employees-Attendance.pdf"
            };
        }

        [Authorize(Permissions.EmployeeAttendance.View)]
        public IActionResult Print([FromQuery(Name = "data-range")] string dateRange)
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
            var EmpsAttendance = _attendanceRepo.GetAll(startDate, endDate);
            return View(EmpsAttendance);
        }

       
    }
}

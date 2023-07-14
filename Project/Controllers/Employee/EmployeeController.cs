using FinalProject.Repository.interfaces;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using FinalProject.Enums;
using FinalProject.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers.Employee
{
    [Authorize]
    public class EmployeeController : Controller
	{
		IEmployeeRepository EmployeeRepo;
		IDepartmentRepository DepartmentRepo;
		IGeneralSettingsRepository GeneralSettingsRepo;
		public EmployeeController(IEmployeeRepository _EmployeeRepo, IDepartmentRepository _Deptrepo, IGeneralSettingsRepository generalSettingsRepo)
		{
			EmployeeRepo = _EmployeeRepo;
			DepartmentRepo = _Deptrepo;
			GeneralSettingsRepo = generalSettingsRepo;
		}


        
        [Authorize(Permissions.Employee.View)]
        public IActionResult Index()
		{
			string responseData = TempData["ResponseData"] as string;
			if (!string.IsNullOrEmpty(responseData)) { ViewData["Response"] = responseData; }

			List<Models.Employee> employeesList = EmployeeRepo.GetAll();
			ViewData["departmentsList"] = DepartmentRepo.GetAll();
			return View(employeesList);
		}
		//////////////////////////////////////////////////////////////////////////////////// Show
		public IActionResult Show(int id)
		{
			Models.Employee selectedEmployee = EmployeeRepo.GetById(id);
			ViewData["department"] = DepartmentRepo.GetById(selectedEmployee.dept_id).Name;
			return View(selectedEmployee);
		}

		//////////////////////////////////////////////////////////////////////////////////// Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Permissions.Employee.Edit)]
        public IActionResult Edit(Models.Employee editedEmployee)
		{
			try
			{
				if (ModelState.IsValid && editedEmployee != null)
				{
					if (editedEmployee.attend_from > editedEmployee.attend_to)////// checking attend_from/to TIME
					{
						TempData["ResponseData"] = "You Entered Attend-From Time That Is After Attend-To Time!";
						return RedirectToAction("Index");
					}

					var AgeValidationDate = DateTime.Today.AddYears(-20);
					if (editedEmployee.birth_date > AgeValidationDate)////// checking Age To Be 20 or more
					{
						TempData["ResponseData"] = "Age Is less than 20!";
						return RedirectToAction("Index");
					}

					var establishmentDate = GeneralSettingsRepo.GetById(1).establishmentDate;
					if (establishmentDate > editedEmployee.hire_date)////// checking hire_date/establishmentDate DATE
					{
						TempData["ResponseData"] = "You Entered Contract Date That Is Before The Establishment Of The Company!";
						return RedirectToAction("Index");
					}

					EmployeeRepo.Update(editedEmployee);
					TempData["ResponseData"] = Responses.success.ToString();
					return RedirectToAction("Index");
				}
				else
				{
					TempData["ResponseData"] = "You Entered Invalid Data | Please Try Again";
					return RedirectToAction("Index");
					//return View("~/Views/Employee/Index.cshtml", editedEmployee);
				}
			}
			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
				return RedirectToAction("Index");
			}
		}

		//////////////////////////////////////////////////////////////////////////////////// New
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Permissions.Employee.Create)]
        public IActionResult New(Models.Employee newEmployee)
		{
			Random random = new Random();
			newEmployee.Ssn = random.Next(100000000, 999999999);
			try
			{
				if (ModelState.IsValid)
				{
					if (newEmployee.attend_from > newEmployee.attend_to)////// checking attend_from/to TIME
					{
						TempData["ResponseData"] = "You Entered Attend-From Time That Is After Attend-To Time!";
						return RedirectToAction("Index");
					}

					var AgeValidationDate = DateTime.Today.AddYears(-20);
					if (newEmployee.birth_date > AgeValidationDate)////// checking Age To Be 20 or more
					{
						TempData["ResponseData"] = "Age Is less than 20!";
						return RedirectToAction("Index");
					}

					var establishmentDate = GeneralSettingsRepo.GetById(1).establishmentDate;
					if (establishmentDate > newEmployee.hire_date)////// checking hire_date/establishmentDate DATE
					{
						TempData["ResponseData"] = "You Entered Contract Date That Is Before The Establishment Of The Company!";
						return RedirectToAction("Index");
					}

					EmployeeRepo.Insert(newEmployee);
					TempData["ResponseData"] = Responses.success.ToString();
					return RedirectToAction("Index");
				}
				else
				{
					TempData["ResponseData"] = "You Entered Invalid Data | Please Try Again";
					return RedirectToAction("Index");
				}
			}

			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
				return RedirectToAction("Index");
			}
		}

		//public JsonResult GetDepartmentList()
		//{
		//    var departmentsList = DepartmentRepo.GetAll();

		//    // Return JSON result
		//    return Json(departmentsList);
		//}


		//public ActionResult GetPartialView()
		//{
		//	var departmentsList = DepartmentRepo.GetAll();
		//	return PartialView("_AddFormPartial", departmentsList);
		//}


		//////////////////////////////////////////////////////////////////////////////////// Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Permissions.Employee.Delete)]
        public IActionResult Delete(Models.Employee selectedEmp)
		{
			try
			{
				if (selectedEmp.Id != null)
				{
					EmployeeRepo.Delete(selectedEmp);
					TempData["ResponseData"] = Responses.success.ToString();
				}
				else
				{
					TempData["ResponseData"] = Responses.fail.ToString();
				}
				return RedirectToAction("Index");
			}
			catch
			{
				TempData["ResponseData"] = Responses.fail.ToString();
				return RedirectToAction("Index");
			}
		}
	}
}

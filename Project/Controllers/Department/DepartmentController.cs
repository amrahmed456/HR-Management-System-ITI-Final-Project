using FinalProject.Repository.interfaces;
using FinalProject.RequestModel;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using Castle.Components.DictionaryAdapter.Xml;
using FinalProject.Enums;
using FinalProject.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Constants;

namespace FinalProject.Controllers.Department
{
    [Authorize]
    public class DepartmentController : Controller
	{
		IDepartmentRepository DepartmentRepository;
		IEmployeeRepository EmployeeRepository;
		public DepartmentController(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository)
		{
			this.DepartmentRepository = departmentRepository;
			this.EmployeeRepository = employeeRepository;
		}





        [Authorize(Permissions.Department.View)]
        public IActionResult Index()
		{
			string responseData = TempData["ResponseData"] as string;
			if (!string.IsNullOrEmpty(responseData))
			{
				ViewData["Response"] = responseData;
			}

			List<Models.Department> departmentsList = DepartmentRepository.GetAll();


			return View(departmentsList);
		}

		//[HttpPost]
		//public IActionResult getDepartments()
		//{
		//	List<Models.Department> departmentsList = DepartmentRepository.GetAll();
		//	return Json(departmentsList);
		//}


		//public IActionResult Index()
		//{
		//	List<DepartmentViewModel> departmentData = new List<DepartmentViewModel>();

		//	var departments = DepartmentRepository.GetAll();
		//	var Employees = EmployeeRepository.GetAll();


		//	foreach (var department in departments)
		//	{
		//		var employeeCount = Employees.Count(e => e.dept_id == department.Id);

		//		var departmentViewModel = new DepartmentViewModel
		//		{
		//			DepartmentId = department.Id,
		//			DepartmentName = department.Name,
		//			EmployeeCount = employeeCount
		//		};

		//		departmentData.Add(departmentViewModel);
		//	}

		//	return View(departmentData);
		//}


		[HttpPost]
        [Authorize(Permissions.Department.Create)]
        public IActionResult New(Models.Department newDepartment)
		{
			try
			{
				if (ModelState.IsValid == true)
				{
					//var department = new Models.Department()
					//{
					//	Name = newDepartment.Name
					//};
					DepartmentRepository.Insert(newDepartment);
					TempData["ResponseData"] = Responses.success.ToString();
				}
			}
			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
			}
			return RedirectToAction("Index");
		}


		[HttpPost]
        [Authorize(Permissions.Department.Edit)]
        public IActionResult Edit(Models.Department editedDep)
		{
			try
			{
				if (ModelState.IsValid == true)
				{
					DepartmentRepository.Update(editedDep);
					TempData["ResponseData"] = Responses.success.ToString();
				}
				else
				{
					TempData["ResponseData"] = Responses.fail.ToString();
				}
			}
			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
			}
			return RedirectToAction("Index");
		}


		//public IActionResult Delete(int Id)
		//{
		//	Models.Department SelectedDep = DepartmentRepository.GetById(Id);
		//	if (SelectedDep != null)
		//	{
		//		DepartmentRepository.Delete(SelectedDep);
		//		return RedirectToAction("Index");
		//	}
		//	return RedirectToAction("Index");
		//}
	}
}

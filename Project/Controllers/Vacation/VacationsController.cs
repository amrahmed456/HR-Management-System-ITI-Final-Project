using FinalProject.Data;
using FinalProject.Enums;
using FinalProject.Migrations;
using FinalProject.Repository.interfaces;
using FinalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Constants;

namespace FinalProject.Controllers.Vacations
{
    [Authorize]
    public class VacationsController : Controller
	{
		IVacationRepository vacationRepo;

		public VacationsController(IVacationRepository _vacationRepo)
		{

			vacationRepo = _vacationRepo;
		}



        //public IActionResult Index()
        //{
        //	var vacations = vacationRepo.GetAll();
        //	string responseData = TempData["ResponseData"] as string;
        //	if (!string.IsNullOrEmpty(responseData))
        //	{
        //		ViewData["Response"] = responseData;
        //	}

        //	return View("~/Views/Vacations/vacationTest.cshtml", vacations);
        //}


        [Authorize(Permissions.Vacations.View)]
        public IActionResult Index()
		{
			var vacations = vacationRepo.GetAll();
			string responseData = TempData["ResponseData"] as string;
			if (!string.IsNullOrEmpty(responseData))
			{
				ViewData["Response"] = responseData;
			}

			return View("~/Views/Vacations/index.cshtml", vacations);
		}

        [Authorize(Permissions.Vacations.Create)]
        public IActionResult Create(VacationViewModel vacationViewModel)
		{

			if (ModelState.IsValid)
			{
				Vacation vacation = new Vacation()
				{ Name = vacationViewModel.Name, Date = vacationViewModel.Date, Created_at = vacationViewModel.Created_at };
				vacationRepo.Insert(vacation);
				vacationRepo.SaveChanges(); // Save changes to the database
				TempData["ResponseData"] = Responses.success.ToString();
			}
			else
			{
				TempData["ResponseData"] = Responses.fail.ToString();
			}

			return RedirectToAction("Index");
		}




        //[HttpPost]
        //public IActionResult Create(VacationViewModel vacationViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid == true)
        //        {
        //            Vacation vacation = new Vacation()
        //            { Name = vacationViewModel.Name, Date = vacationViewModel.Date, Created_at = vacationViewModel.Created_at };
        //            vacationRepo.Insert(vacation);
        //            vacationRepo.SaveChanges(); // Save changes to the database
        //            TempData["ResponseData"] = Responses.success.ToString();
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["ResponseData"] = Responses.fail.ToString();
        //            return View(vacationViewModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ResponseData"] = Responses.fail.ToString();
        //        return RedirectToAction("Index");
        //    }
        //}


        [Authorize(Permissions.Vacations.Delete)]
        public IActionResult Delete(int id)
		{
			try
			{
				Vacation vacation = vacationRepo.GetById(id);
				vacationRepo.Delete(vacation);
				vacationRepo.SaveChanges();
				TempData["ResponseData"] = Responses.success.ToString();
			}catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
			}

			return RedirectToAction("Index");
		}


        public IActionResult Edit()
		{
			return View();
		}





	}
}

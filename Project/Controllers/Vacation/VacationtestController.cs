using FinalProject.Models;
using FinalProject.Repository.interfaces;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Drawing.Printing;
using FinalProject.Models;
using FinalProject.Enums;
using FinalProject.ViewModel;

namespace FinalProject.Controllers.Vacations
{

    public class VacationtestController : Controller
    {
        IVacationRepository vacationRepo;

        public VacationtestController(IVacationRepository _vacationRepo)
        {

            vacationRepo = _vacationRepo;
        }

        public IActionResult Index(int page = 1, int pageSize = 3 ,bool HasPrevicePage=true , bool HasNextPage = true)
        {
            var vacations = vacationRepo.GetAll()
                .OrderByDescending(v => v.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalRecords = vacationRepo.GetAll().Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var model = new PaginatedList<Vacation>(vacations, page, pageSize, totalRecords, totalPages,  HasPrevicePage , HasNextPage);

           

            string responseData = TempData["ResponseData"] as string;
            if (!string.IsNullOrEmpty(responseData))
            {
                ViewData["Response"] = responseData;
            }

            return View(model);
        }


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




		public IActionResult Delete(int id)
		{
			try
			{
				Vacation vacation = vacationRepo.GetById(id);
				vacationRepo.Delete(vacation);
				vacationRepo.SaveChanges();
				TempData["ResponseData"] = Responses.success.ToString();
			}
			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
			}

			return RedirectToAction("Index");
		}




	}



}

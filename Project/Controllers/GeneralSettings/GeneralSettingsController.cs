using FinalProject.Constants;
using FinalProject.Enums;
using FinalProject.Models;
using FinalProject.Repository.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers.GeneralSettings
{
    [Authorize]
    public class GeneralSettingsController : Controller
	{
		private IGeneralSettingsRepository GeneralSettingsRepository;
		public GeneralSettingsController(IGeneralSettingsRepository GeneralSettingsRepo)
		{
			GeneralSettingsRepository = GeneralSettingsRepo;

		}

        [Authorize(Permissions.GeneralSettings.Edit)]
        public IActionResult Index()
		{
			string responseData = TempData["ResponseData"] as string;
			if (!string.IsNullOrEmpty(responseData))
			{
				ViewData["Response"] = responseData;
			}

			Models.GeneralSettings generalSettingsModel = GeneralSettingsRepository.GetById(1); // only one row will be used (with Id=1)
			return View(generalSettingsModel);
		}


		[HttpPost]
		public IActionResult Save(Models.GeneralSettings newSettings)
		{
			try
			{
				if (ModelState.IsValid == true && newSettings.weekly_vacation1 != newSettings.weekly_vacation2)
				{
					TempData["ResponseData"] = Responses.success.ToString();
					GeneralSettingsRepository.Update(newSettings);
					return RedirectToAction("Index");
				}
				else
				{
					TempData["ResponseData"] = Responses.fail.ToString();
					return RedirectToAction("Index", newSettings);
				}
			}
			catch (Exception ex)
			{
				TempData["ResponseData"] = Responses.fail.ToString();
				return RedirectToAction("Index");
			}
		}
	}
}

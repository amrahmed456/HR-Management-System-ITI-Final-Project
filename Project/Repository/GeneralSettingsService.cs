using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository
{
	public class GeneralSettingsService : IGeneralSettingsRepository
	{
		private readonly HRContext context;
		public GeneralSettingsService(HRContext context)
		{
			this.context = context;
		}

		public GeneralSettings GetById(int id)
		{
			try
			{
				//checking if there is any current changes otherwise set defaults
				GeneralSettings currentGeneralSettings = context.GeneralSettings.Find(1);
				if (currentGeneralSettings == null ||
					currentGeneralSettings.Id != 1 ||
					currentGeneralSettings.add_hours == null ||
					currentGeneralSettings.sub_hours == null ||
					currentGeneralSettings.weekly_vacation1 == null)
				{
					GeneralSettings defaultGeneralSettings = new GeneralSettings();
					//defaultGeneralSettings.Id = 1;
					defaultGeneralSettings.add_hours = 1;
					defaultGeneralSettings.sub_hours = 1;
					defaultGeneralSettings.weekly_vacation1 = "friday";
					defaultGeneralSettings.establishmentDate = new DateTime(2022, 12, 1);
					Insert(defaultGeneralSettings);
				}
				return context.GeneralSettings.Find(1);
			}
			catch (Exception ex)
			{
				return context.GeneralSettings.Find(1);
			}
		}

		public void Update(GeneralSettings newSettings)
		{
			//oldSetting.add_hours = newSettings.add_hours;
			//oldSetting.sub_hours = newSettings.sub_hours;
			//oldSetting.weekly_vacation1 = newSettings.weekly_vacation1;
			//oldSetting.weekly_vacation2 = newSettings.weekly_vacation2;
			newSettings.Id = 1;
			context.GeneralSettings.Update(newSettings);
			context.SaveChanges();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public List<GeneralSettings> GetAll()
		{
			throw new NotImplementedException();
		}

		public void Delete(GeneralSettings t)
		{
			throw new NotImplementedException();
		}

		public void Insert(GeneralSettings newSettings)
		{
			//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.GeneralSettings ON");
			context.GeneralSettings.Add(newSettings);
			//context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.GeneralSettings OFF");
			context.SaveChanges();
		}
	}
}

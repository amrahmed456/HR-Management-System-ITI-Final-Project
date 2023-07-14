using FinalProject.Models;

namespace FinalProject.Repository.interfaces
{
	public interface IVacationRepository:IRepository<Vacation>
	{
		void SaveChanges();
		Vacation GetVacationOfToday();
	}
}

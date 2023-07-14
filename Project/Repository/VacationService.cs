using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;

namespace FinalProject.Repository
{
	public class VacationService : IVacationRepository
	{
		HRContext Context;
        public VacationService(HRContext _Context)
        {
          Context = _Context;
        }
        public void Delete(Vacation t)
		{
			Context.Vacations.Remove(t);
		}

		public List<Vacation> GetAll()
		{
			 return Context.Vacations.ToList();
		}

		public Vacation GetById(int id)
		{
			return Context.Vacations.FirstOrDefault(v => v.Id == id);
		}

		public void Insert(Vacation t)
		{
			Context.Vacations.Add(t);
			Context.SaveChanges();
		}

		public void SaveChanges()
		{
			Context.SaveChanges();	
		}

		public void Update(Vacation t)
		{
			Context.Vacations.Update(t);
			Context.SaveChanges();
		}

		public Vacation? GetVacationOfToday()
		{
            return Context.Vacations.Where(v => v.Date.Date == DateTime.Now.Date).FirstOrDefault();
        }

    }
}

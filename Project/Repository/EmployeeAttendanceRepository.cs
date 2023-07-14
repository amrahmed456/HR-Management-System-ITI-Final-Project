using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository
{
	public class EmployeeAttendanceRepository : IEmployeeAttendanceRepository
	{

		HRContext context;
		public EmployeeAttendanceRepository(HRContext _Context)
		{
			context = _Context;
		}

		public void Delete(EmployeeAttendance t)
		{
			context.EmployeeAttendance.Remove(t);
			context.SaveChanges();
		}

		public List<EmployeeAttendance> GetAll()
		{
            return context.EmployeeAttendance.OrderByDescending(ea => ea.CreatedAt).ToList();
        }

        public List<EmployeeAttendance> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var query = context.EmployeeAttendance.OrderByDescending(ea => ea.CreatedAt);

            if (startDate.HasValue)
            {
                query = (IOrderedQueryable<EmployeeAttendance>)query.Where(ea => ea.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = (IOrderedQueryable<EmployeeAttendance>)query.Where(ea => ea.CreatedAt <= endDate.Value);
            }

            return query.ToList();
        }

        public EmployeeAttendance GetById(int id)
		{
			return context.EmployeeAttendance.FirstOrDefault(e => e.Id == id);
		}

		public void Insert(EmployeeAttendance t)
		{
			context.EmployeeAttendance.Add(t);
			context.SaveChanges();
		}

        public void Update(EmployeeAttendance t, EmployeeAttendance o)
		{
            o.CheckIn = t.CheckIn;
            o.CheckOut = t.CheckOut;
            o.OverTimeHours = t.OverTimeHours;
            o.OverTimeValue = t.OverTimeValue;
            o.DeductionHours = t.DeductionHours;
            o.DeductionValue = t.DeductionValue;
            o.Amount = t.Amount;

            // Save the changes to the database
            context.SaveChanges();
        }


        public void Update(EmployeeAttendance t)
		{
            context.EmployeeAttendance.Update(t);
			context.SaveChanges();
		}

        public List<Employee> GetEmployeesThatDidntAttendForToday()
        {
			var query = from emp in context.Employees
						where !context.EmployeeAttendance.Any(att => att.EmployeeId == emp.Id && att.CreatedAt.Date == DateTime.Today)
							  && emp.IsDeleted == false
						select emp;

			return query.ToList();
		}

        public bool IsAttendanceTakenForToday(int ssn)
        {
           
            var date = DateTime.Now;
            var attendance = context.EmployeeAttendance
                .FirstOrDefault(a => a.Employee.Ssn == ssn && a.CreatedAt.Date == date.Date);

             return attendance != null;
            
        }

        public List<EmployeeAttendance> GetAttendanceForToday(int limit = 6)
        {
            var date = DateTime.Now.Date;
            return context.EmployeeAttendance
                .Where(a => a.CreatedAt.Date == date)
                .OrderByDescending(a => a.CreatedAt)
                .Take(limit)
                .ToList();
        }
    }
}

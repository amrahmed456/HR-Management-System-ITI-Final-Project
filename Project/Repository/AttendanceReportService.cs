using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;

namespace FinalProject.Repository
{
    public class AttendanceReportService : IAttendanceReportRepository
    {

        private readonly HRContext context;

        public AttendanceReportService(HRContext context)
        {
            this.context = context;
        }

        public void Delete(AttendanceReport t)
        {
            throw new NotImplementedException();
        }

        public List<AttendanceReport> GetAll()
        {
            return context.AttendanceReport.OrderByDescending(r => r.CreatedAt).ToList();
        }

		public List<AttendanceReport> GetAll(DateTime? startDate, DateTime? endDate)
		{
			var query = context.AttendanceReport.OrderByDescending(ea => ea.CreatedAt);

			if (startDate.HasValue)
			{
				query = (IOrderedQueryable<AttendanceReport>)query.Where(ea => ea.CreatedAt >= startDate.Value);
			}

			if (endDate.HasValue)
			{
				query = (IOrderedQueryable<AttendanceReport>)query.Where(ea => ea.CreatedAt <= endDate.Value);
			}

			return query.ToList();
		}

		public AttendanceReport GetById(int id)
        {
            return context.AttendanceReport.FirstOrDefault(r => r.Id == id);
        }

        public void Insert(AttendanceReport t)
        {
            context.AttendanceReport.Add(t);
            context.SaveChanges();
        }

        public void Update(AttendanceReport t)
        {
            throw new NotImplementedException();
        }

        public AttendanceReport GetEmployeeAttendanceReportForThisMonthOfTheYear(int empId)
        {
            var currentDate = DateTime.Now;
            var report = context.AttendanceReport.Where(
                a => a.EmployeeId == empId &&
                a.CreatedAt.Year == currentDate.Year &&
                a.CreatedAt.Month == currentDate.Month).FirstOrDefault();

            if (report == null)
            {
                // create new one
                report = new AttendanceReport()
                {
                    EmployeeId = empId
                };
                Insert(report);
            }

            return report;
        }
    }
}

using FinalProject.Models;

namespace FinalProject.Repository.interfaces
{
    public interface IAttendanceReportRepository : IRepository<AttendanceReport>
    {
        public AttendanceReport GetEmployeeAttendanceReportForThisMonthOfTheYear(int empId);
		public List<AttendanceReport> GetAll(DateTime? startDate, DateTime? endDate);
	}
}

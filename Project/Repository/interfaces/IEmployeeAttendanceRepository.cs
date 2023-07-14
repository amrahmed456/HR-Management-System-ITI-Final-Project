using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Repository.interfaces
{
	public interface IEmployeeAttendanceRepository : IRepository<EmployeeAttendance>
	{
		public List<Employee>? GetEmployeesThatDidntAttendForToday();
		public void Update(EmployeeAttendance t,EmployeeAttendance o);
		public List<EmployeeAttendance> GetAll(DateTime? startDate, DateTime? endDate);
		public bool IsAttendanceTakenForToday(int ssn);
		public List<EmployeeAttendance> GetAttendanceForToday(int limit = 6);
    }
}

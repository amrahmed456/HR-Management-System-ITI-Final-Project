using FinalProject.Data;
using FinalProject.Repository;
using FinalProject.Repository.interfaces;
using FinalProject.ViewModel;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FinalProject.Controllers.Attendance
{
	public class AttendanceCronJobs
	{

		private readonly IServiceProvider _serviceProvider;

		public AttendanceCronJobs(
			IServiceProvider serviceProvider
		)
		{
			_serviceProvider = serviceProvider;
		}


		public void ScheduleJobs()
		{
			
			RecurringJob.AddOrUpdate("CheckIfTodayIsVacationForAttendance", () => CheckIfTodayIsVacationForAttendance(), "1 0 0 * * ?");
            RecurringJob.AddOrUpdate("CheckForAbsentEmployees", () => CheckForAbsentEmployees(), "58 23 * * * ?");
        }

		public void CheckIfTodayIsVacationForAttendance()
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var _attendanceRepo = scope.ServiceProvider.GetRequiredService<IEmployeeAttendanceRepository>();
				var _gsRepo = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
				var _vacationRepo = scope.ServiceProvider.GetRequiredService<IVacationRepository>();
				var holiday = _vacationRepo.GetVacationOfToday();
				var gs = _gsRepo.GetById(1);
				string todayName = DateTime.Today.DayOfWeek.ToString().ToLower();
				bool isVacation = false;

				if (gs.weekly_vacation1 == todayName || gs.weekly_vacation2 == todayName)
				{
					isVacation = true;
				}
				if (holiday != null)
				{
					isVacation = true;
				}

				if (isVacation)
				{
					var employees = _attendanceRepo.GetEmployeesThatDidntAttendForToday();
					foreach (var emp in employees)
					{
						var attendance = new AttendanceFormViewModel()
						{
							Ssn = emp.Ssn,
							CheckIn = DateTime.Now,
							CheckOut = DateTime.Now
						};
						InsertNewAttendance(attendance,false,emp);
					}
				}
			}
			
			
			
		}

		public bool InsertNewAttendance(AttendanceFormViewModel attendance, bool isAbsent = false , Models.Employee employee = null)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var _attendanceRepo = scope.ServiceProvider.GetRequiredService<IEmployeeAttendanceRepository>();
				var _employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
				var _gsRepo = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
				var _vacationRepo = scope.ServiceProvider.GetRequiredService<IVacationRepository>();
                var _reportRepo = scope.ServiceProvider.GetRequiredService<IAttendanceReportRepository>();

				if (employee != null)
				{
					// check if employee already took the attendance for today
					var gs = _gsRepo.GetById(1);
					AttendanceHelper.officialHoliday = _vacationRepo.GetVacationOfToday();
                    AttendanceHelper.Report = _reportRepo.GetEmployeeAttendanceReportForThisMonthOfTheYear(employee.Id);
                    var EmplAttendance = AttendanceHelper.CalculateAllRequiredParameters(attendance, gs, employee,false, isAbsent);
					_attendanceRepo.Insert(EmplAttendance);
					return true;
				}

				return false;
			}
			
		}

		public void CheckForAbsentEmployees()
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var _attendanceRepo = scope.ServiceProvider.GetRequiredService<IEmployeeAttendanceRepository>();
				
				var employees = _attendanceRepo.GetEmployeesThatDidntAttendForToday();
				foreach (var emp in employees)
				{
					var attendance = new AttendanceFormViewModel()
					{
						Ssn = emp.Ssn,
						CheckIn = DateTime.Now,
						CheckOut = DateTime.Now
					};
					InsertNewAttendance(attendance, true , emp);
				}
			}
				
		}
	}
}
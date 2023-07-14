using FinalProject.Data;
using FinalProject.Migrations;
using FinalProject.Models;
using FinalProject.ViewModel;

namespace FinalProject.Controllers.Attendance
{
    public static class AttendanceHelper
    {
        
        public static EmployeeAttendance EditModel;
        public static Vacation officialHoliday;
        public static AttendanceReport Report;

        public static double CalculateSalaryPerHour(Models.Employee employee, Models.GeneralSettings gs)
        {
            double hoursePerDay = (employee.attend_to - employee.attend_from).TotalHours;

            int numOfWeeklyHolidays = (gs.weekly_vacation2 != null) ? 8 : 4;
            double salaryPerHour = (double)employee.salary/ (hoursePerDay * (30- numOfWeeklyHolidays));
            return salaryPerHour;
        }

        public static EmployeeAttendance CalculateAllRequiredParameters
        (
            AttendanceFormViewModel attendance,
            Models.GeneralSettings gs,
            Models.Employee employee,
            bool edit = false,
            bool isAbsent = false
        )
        {
            double salaryPerHour = CalculateSalaryPerHour(employee,gs);
            double amount = 0;
            bool isVacation = false;
            string vacationName = null;
            var attend_from = employee.attend_from;
            var attend_to = employee.attend_to;
            TimeSpan overTimeHours = TimeSpan.Zero;
            double overTimeValue = 0;
            TimeSpan deductionHours = TimeSpan.Zero;
            double deductionValue = 0;
            bool calculateAmount = true;

            // check if is weekly holiday
            if (!edit)
            {
                string todayName = DateTime.Today.DayOfWeek.ToString().ToLower();
                if (gs.weekly_vacation1 == todayName || gs.weekly_vacation2 == todayName)
                {
                    isVacation = true;
                    calculateAmount = false;
                    vacationName = "Weekly Holiday: " + todayName;
                }

                if (!isVacation)
                {
                    // check for a paid vacation
                    if (officialHoliday != null)
                    {
                        isVacation = true;
                        attendance.CheckIn = employee.attend_from;
                        attendance.CheckOut = employee.attend_to;
                        vacationName = officialHoliday.Name;

                    }
                }
            }
            else
            {
                isVacation = EditModel.IsVacation;
                vacationName = EditModel.VacationName;
                salaryPerHour = EditModel.SalaryPerHour;
                attend_from = EditModel.AttendForm;
                attend_to = EditModel.AttendTo;
            }

            if (calculateAmount)
            {
                overTimeHours = attendance.CheckOut.TimeOfDay - attend_to.TimeOfDay;
                overTimeHours = (overTimeHours > TimeSpan.Zero) ? overTimeHours : TimeSpan.Zero;

                overTimeValue = overTimeHours.TotalHours * gs.add_hours * salaryPerHour;

                TimeSpan postSubHours = attendance.CheckOut.TimeOfDay - attend_to.TimeOfDay;
                postSubHours = (postSubHours < TimeSpan.Zero) ? -postSubHours : TimeSpan.Zero;

                TimeSpan preSubHours = attend_from.TimeOfDay - attendance.CheckIn.TimeOfDay;
                preSubHours = (preSubHours < TimeSpan.Zero) ? -preSubHours : TimeSpan.Zero;

                deductionHours = postSubHours + preSubHours;

                deductionValue = deductionHours.TotalHours * gs.sub_hours * salaryPerHour;

                amount = salaryPerHour * (attend_to.TimeOfDay - attend_from.TimeOfDay).TotalHours + overTimeValue - deductionValue;

            }

            amount = isAbsent ? 0 : amount;
            overTimeHours = (amount == 0) ? TimeSpan.Zero : overTimeHours;
			overTimeValue = (amount == 0) ? 0 : overTimeValue;
			deductionHours = (amount == 0) ? TimeSpan.Zero : deductionHours;
			deductionValue = (amount == 0) ? 0 : deductionValue;


			var EmplAttendance = new EmployeeAttendance()
            {
                IsVacation = isVacation,
                VacationName = vacationName,
                EmployeeId = employee.Id,
                Department = employee.Department.Name,
                Salary = (double)employee.salary,
                SalaryPerHour = salaryPerHour,
                AttendForm = attend_from,
                AttendTo = attend_to,
                CheckIn = attendance.CheckIn,
                CheckOut = attendance.CheckOut,
                OverTimeHours = overTimeHours,
                OverTimeValue = overTimeValue,
                DeductionHours = deductionHours,
                DeductionValue = deductionValue,
                Amount = (amount < 0) ? 0 : amount,
                AttendanceReportId = Report.Id
            };

            return EmplAttendance;
        }

       
    }
}

using FinalProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModel
{
	public class ReportsViewModel
	{
		//Employee Name
		//	Department
		//	Salary
		//	Attend days
		//	Absence days
		//	Additional Hours 
		//	Deduction Hours
		//	Total Deduction
		//	Total overtime
		//	Net Salary
		//	Month
		//year

		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }

		public string Department { get; set; }

		public double Salary { get; set; }

		public int AttendDays { get; set; }
		public int AbsenceDays { get; set; }

	[Column(TypeName = "time")]
		public TimeSpan OverTimeHours { get; set; }

		public double OverTimeValue { get; set; }

		[Column(TypeName = "time")]
		public TimeSpan DeductionHours { get; set; }

		public decimal TotalDeductionHours { get; set; }

        public decimal TotalOvertimeHours { get; set; }


        public double DeductionValue { get; set; }

		public double NetSalary { get; set; }

		public int Month { get; set; }

		public int Year { get; set; }

	}

}






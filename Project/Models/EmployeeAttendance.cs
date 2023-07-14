using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace FinalProject.Models
{
	public class EmployeeAttendance
	{
		[Key]
        public int Id { get; set; }

        public bool IsVacation { get; set; }

        public string? VacationName { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public string Department { get; set; }

        public double Salary { get; set; }

        public double SalaryPerHour { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		public DateTime AttendForm { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		public DateTime AttendTo { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		public DateTime CheckIn { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		public DateTime CheckOut { get; set; }

		[Column(TypeName = "time")]
		public TimeSpan OverTimeHours { get; set; }

        public double OverTimeValue { get; set; }

        [Column(TypeName = "time")]
		public TimeSpan DeductionHours { get; set; }

        public double DeductionValue { get; set; }

        public double Amount { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("AttendanceReport")]
        public int AttendanceReportId { get; set; }
        public virtual AttendanceReport? AttendanceReport { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModel
{
	public class AttendanceFormViewModel
	{
		public int Ssn { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		public DateTime CheckIn { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
		[CheckOutTimeGreaterThanCheckInTime(ErrorMessage = "CheckOut time must be greater than CheckIn time.")]
		public DateTime CheckOut { get; set; }

	}


	public class CheckOutTimeGreaterThanCheckInTimeAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var checkOutTime = (DateTime)value;
			var checkInTime = (DateTime)validationContext.ObjectType.GetProperty("CheckIn").GetValue(validationContext.ObjectInstance, null);

			if (checkOutTime <= checkInTime)
			{
				return new ValidationResult(ErrorMessage);
			}

			return ValidationResult.Success;
		}
	}

    public class EmployeeAttendanceFormModel{
        public int Ssn { get; set; }
		public string Name { get; set; }
        public string DepartmentName { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
	public class GeneralSettings
	{
		[Key]
		public int Id { get; set; }


		[Required(ErrorMessage = "The Additional Hour field is required.")]
		[Range(1, 10, ErrorMessage = "Value must be between 1 and 10.")]
		public double add_hours{ get; set; }


		[Required(ErrorMessage = "The late Hour field is required.")]
		[Range(1, 10, ErrorMessage = "Value must be between 1 and 10.")]
		public double sub_hours { get; set; }


		[Required]
		[RegularExpression("^(monday|tuesday|wednesday|thursday|friday|saturday|sunday)$", ErrorMessage = "Please enter at least one valid day of the week.")]
		public string weekly_vacation1 { get; set;}

		// disabled RegularExpression if user wanted to choose only one day
		//[RegularExpression("^(monday|tuesday|wednesday|thursday|friday|saturday|sunday)$", ErrorMessage = "Please enter a valid day of the week.")]
		public string? weekly_vacation2 { get; set;}

		[Required]
		[DataType(DataType.Date)]
		public DateTime establishmentDate { get; set; }
	}
}

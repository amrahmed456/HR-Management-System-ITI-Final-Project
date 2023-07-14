using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModel
{
	public class VacationViewModel
	{
		[Required]
		public string Name { set; get; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }


		[DataType(DataType.Date)]
		public DateTime? Created_at { get; set; } = DateTime.Now;
	}
}

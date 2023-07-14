using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProject.Models
{
	public class Vacation
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { set; get; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }


		[DataType(DataType.Date)]
		public DateTime? Created_at { get; set; } = DateTime.Now;


	}
}

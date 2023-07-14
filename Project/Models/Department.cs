using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
	public class Department
	{
		[Key]
		public int Id { get; set; }


		[Required]
		public string Name { get; set; }


		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public virtual List<Employee>? Employee { get; set; }

	}
}

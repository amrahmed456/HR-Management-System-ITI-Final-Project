using FinalProject.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        //[Index(IsUnique = true)] made it using fluent api
        public int Ssn { get; set; }

        [MinLength(3)]
        public string Name { get; set; }
        public string address { get; set; }

        [StringLength(11)]
        public string? phone { get; set; }

        [RegularExpression("^(male|female)$", ErrorMessage = "Please Select The Gender ( Male Or Female ).")]
        public string gender { get; set; }
        public string nationality { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birth_date { get; set; }

        [StringLength(14)]
        public string? national_number { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid Salary")]
        public double salary { get; set; }

        public string job_title { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime hire_date { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime attend_from { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime attend_to { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime created_at { get; set; } = DateTime.Now;

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? deleted_at { get; set; }

        [ForeignKey("Department")]
        public int dept_id { get; set; }

        public virtual Department? Department { get; set; } 

        public virtual List<EmployeeAttendance>? EmployeeAttendances { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual List<AttendanceReport>? AttendanceReport { get; set; }
    }
}

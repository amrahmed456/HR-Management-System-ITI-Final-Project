using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinalProject.Data
{
	public class HRContext : IdentityDbContext<ApplicationUser>
    {
		
		public HRContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Ssn)
                .IsUnique();
            base.OnModelCreating(modelBuilder);

        }

        public HRContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Vacation> Vacations { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<GeneralSettings> GeneralSettings { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<EmployeeAttendance> EmployeeAttendance { get;set ; }
        public DbSet<AttendanceReport> AttendanceReport { get; set; }
    }
}

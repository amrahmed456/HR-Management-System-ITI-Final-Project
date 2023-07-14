using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;

namespace FinalProject.Repository
{
	public class DepartmentService : IDepartmentRepository
	{

		private readonly HRContext context;

		public DepartmentService(HRContext context)
		{
			this.context = context;
		}

		public List<Department> GetAll()
		{
			return context.Departments.ToList();
		}

		public Department GetById(int id)
		{
			return context.Departments.Find(id);
		}

		public void Update(Department editedDep)
		{
			// Get the department to edit from the database
			var oldDepartment = GetById(editedDep.Id);
			// Update the department properties with the new values
			oldDepartment.Name = editedDep.Name;
			// Save the changes
			context.Departments.Update(oldDepartment);
			context.SaveChanges();
		}

		public void Insert(Department newDepartment)
		{
			context.Departments.Add(newDepartment);
			context.SaveChanges();
		}

		public void Delete(Department t)
		{
			context.Departments.Remove(t);
			context.SaveChanges();
		}

		public int Count(int depId)
		{
			return context.Employees.Count();
		}

        public int Count()
        {
            return context.Departments.Count();
        }
    }
}

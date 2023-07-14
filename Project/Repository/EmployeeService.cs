using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Repository.interfaces;

namespace FinalProject.Repository
{
    public class EmployeeService : IEmployeeRepository
    {

        HRContext context;
        public EmployeeService(HRContext _Context)
        {
            context = _Context;
        }

        public void Delete(Employee selectedEmp)
		{
			//context.Employees.Remove(selectedEmp);  used soft delete
			Employee employeeToDelete = GetById(selectedEmp.Id);
			employeeToDelete.IsDeleted = true;
			Update(employeeToDelete);
			context.SaveChanges();
        }

        public List<Employee> GetAll()
        {
            return context.Employees.Where(e=>e.IsDeleted == false).ToList();
        }

        public Employee GetById(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee == null || employee.IsDeleted == true)
            {
				throw new ArgumentException("Employee not found or deleted");
			}
			return employee;

		}

        public void Insert(Employee newEmployee)
        {
			context.Employees.Add(newEmployee);
            context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            context.Employees.Update(employee);
            context.SaveChanges();
        }

        public Employee GetBySsn(int ssn)
        {
            return context.Employees.FirstOrDefault(e => e.Ssn == ssn && e.IsDeleted == false);
        }

        public object Entry(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void Save()               //saveChanges
        {
            context.SaveChanges();
        }

        public int Count()
        {
            return context.Employees.Where(e => e.IsDeleted == false).Count();
        }

        public int getUsersCount()
        {
            return context.Users.Count();
        }
    }
}

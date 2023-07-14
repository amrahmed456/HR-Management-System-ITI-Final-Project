using FinalProject.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Policy;

namespace FinalProject.Repository.interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        //int Count(Func<Employee, bool> predicate);
        public Employee GetBySsn(int ssn);

        object Entry(Employee employee);

        public void Save();   //saveChanges

        public int Count();

        public int getUsersCount();

    }
}
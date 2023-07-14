using FinalProject.Models;

namespace FinalProject.Repository.interfaces
{
	public interface IDepartmentRepository : IRepository<Department>
	{
		int Count(int depId);
        public int Count();
    }
}

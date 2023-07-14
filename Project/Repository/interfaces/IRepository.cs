namespace FinalProject.Repository.interfaces
{
	public interface IRepository<T>
	{
		List<T> GetAll();
		void Insert(T t);
		void Update(T t);
		void Delete(T t);
		T GetById(int id);
	}
}

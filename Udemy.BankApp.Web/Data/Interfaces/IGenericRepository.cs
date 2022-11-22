namespace Udemy.BankApp.Web.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class,new()
    {
        void Create(T Entity);
        void Remove(T Entity);
        List<T> GetAll();
        T GetById(object id);
        void Update(T Entity);
        IQueryable<T> GetQueryable();
    }
}

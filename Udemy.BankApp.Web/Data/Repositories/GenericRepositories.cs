using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Interfaces;

namespace Udemy.BankApp.Web.Data.Repositories
{
    public class GenericRepositories<T> : IGenericRepository<T> where T : class,new()
    {
        private readonly BankContext _context;

        public GenericRepositories(BankContext context)
        {
            _context = context;
        }

        public void Create(T Entity)
        {
            _context.Set<T>().Add(Entity);
        }
        public void Remove(T Entity)
        {
            _context.Set<T>().Remove(Entity);
        }
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Update(T Entity)
        {
            _context.Set<T>().Update(Entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}

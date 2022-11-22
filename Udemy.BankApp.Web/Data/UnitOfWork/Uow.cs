using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;

namespace Udemy.BankApp.Web.Data.UnitOfWork
{
    public class Uow:IUow
    {
        private readonly BankContext _context;

        public Uow(BankContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> GetGenericRepository<T>() where T : class,new()
        {
            return new GenericRepositories<T>(_context);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

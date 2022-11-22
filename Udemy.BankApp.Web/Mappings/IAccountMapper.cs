using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Models;

namespace Udemy.BankApp.Web.Mappings
{
    public interface IAccountMapper
    {
        public Account Map(AccountCreateModel model);
    }
}

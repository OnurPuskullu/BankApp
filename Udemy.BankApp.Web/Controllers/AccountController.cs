using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mappings;
using Udemy.BankApp.Web.Models;

namespace Udemy.BankApp.Web.Controllers
{
    public class AccountController : Controller
    {
        //private readonly ApplicationUserRepository _applicationUserRepository;
        //private readonly IAccountRepository _accountRepository;
        //private readonly IUserMapper _userMapper;
        //private readonly IAccountMapper _accountMapper;
        //public AccountController(IUserMapper userMapper, ApplicationUserRepository applicationUserRepository, IAccountRepository accountRepository, IAccountMapper accountMapper)
        //{

        //    _userMapper = userMapper;
        //    _applicationUserRepository = applicationUserRepository;
        //    _accountRepository = accountRepository;
        //    _accountMapper = accountMapper;
        //}

        //private readonly IGenericRepository<Account> _accountGenericRepository;
        //private readonly IGenericRepository<ApplicationUser> _userGenericRepository;
        //public AccountController(IGenericRepository<Account> accountGenericRepository, IGenericRepository<ApplicationUser> userGenericRepository)
        //{
        //    _accountGenericRepository = accountGenericRepository;
        //    _userGenericRepository = userGenericRepository;
        //}

        private readonly IUow _uow;

        public AccountController(IUow uow)
        {
            this._uow = uow;
        }

        public IActionResult Create(int id)
        {
            var userInfo = _uow.GetGenericRepository<ApplicationUser>().GetById(id);
            return View(new UserListModel
            {
                Id = userInfo.Id,
                Name = userInfo.Name,
                Surname = userInfo.Surname
            });
        }
        [HttpPost]
        public IActionResult Create(AccountCreateModel model)
        {
            _uow.GetGenericRepository<Account>().Create(new Account
            {
                AccountNumber = model.AccountNumber,
                Balance = model.Balance,
                ApplicationUserId = model.ApplicationUserId
            });
            _uow.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult GetByUserId(int userId)
        {
            var query = _uow.GetGenericRepository<Account>().GetQueryable();
            var accounts=query.Where(x=>x.ApplicationUserId==userId).ToList();   
            var user=_uow.GetGenericRepository<ApplicationUser>().GetById(userId);
            ViewBag.FullName =user.Name + " " + user.Surname;
            var list = new List<AccountListModel>();
            foreach (var account in accounts)
            {
                list.Add(new()
                {
                    AccountNumber = account.AccountNumber,
                    ApplicationUserId = account.ApplicationUserId,
                    Balance = account.Balance,                  
                    Id = account.Id
                });
            }
            return View(list);
        }
        [HttpGet]
        public IActionResult SendMoney(int accountId)
        {
            var query=_uow.GetGenericRepository<Account>().GetQueryable();
            var accounts=query.Where(x=>x.Id!=accountId).ToList();
            var list = new List<AccountListModel>();
            ViewBag.SenderId=accountId;
            foreach (var account in accounts)
            {
                list.Add(new()
                {
                    AccountNumber = account.AccountNumber,
                    ApplicationUserId = account.ApplicationUserId,
                    Balance = account.Balance,
                    Id = account.Id
                });         
            }
            return View(new SelectList(list,"Id","AccountNumber"));
        }
        [HttpPost]
        public IActionResult SendMoney(SendMoneyModel model)
        {
            var senderAccount = _uow.GetGenericRepository<Account>().GetById(model.SenderId);
            senderAccount.Balance -= model.Amount;
            _uow.GetGenericRepository<Account>().Update(senderAccount);

            var account=_uow.GetGenericRepository<Account>().GetById(model.AccountId);
            account.Balance+=model.Amount;
            _uow.GetGenericRepository<Account>().Update(account);

            _uow.SaveChanges();
            return RedirectToAction("Index","Home");   
        }
    }
}

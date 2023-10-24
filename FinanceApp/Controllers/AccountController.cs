using FinanceApp.Models;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FinanceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryAccountTypes _repositoryAccountTypes;
        private readonly IUserService _userService;
        private readonly IRepositoryAccounts _repositoryAccounts;

        public AccountController(IRepositoryAccountTypes repositoryAccountTypes,
            IUserService userService,
            IRepositoryAccounts repositoryAccounts) 
        {
            _repositoryAccountTypes = repositoryAccountTypes;
            _userService = userService;
            _repositoryAccounts = repositoryAccounts;
        }
        [HttpGet]
        public async Task<IActionResult> Create() {
            var userId = _userService.GetUserId();
            var accountTypes = await _repositoryAccountTypes.Get(userId);
            var model = new AccountCreationViewModel();
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreationViewModel account)
        {
            var userId = _userService.GetUserId();
            var accountType = await _repositoryAccountTypes.GetById(account.Id, userId);
            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes(userId);
                return View(account);
            }
            await _repositoryAccounts.Create(account);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var accountsWithAccountType = await _repositoryAccounts.Search(userId);
            var model = accountsWithAccountType.GroupBy(x => x.AccountType)
                .Select(g => new IndexAccountsViewModel
                {
                    AccountType = g.Key,
                    Accounts = g.AsEnumerable()
                }).ToList();
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _repositoryAccountTypes.Get(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}

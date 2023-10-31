using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountController(IRepositoryAccountTypes repositoryAccountTypes,
            IUserService userService,
            IRepositoryAccounts repositoryAccounts,
            IMapper mapper) 
        {
            _repositoryAccountTypes = repositoryAccountTypes;
            _userService = userService;
            _repositoryAccounts = repositoryAccounts;
            _mapper = mapper;
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
        
        [HttpPost]
        public async Task<IActionResult> Update(AccountCreationViewModel accountUpdate)
        {
            var userId = _userService.GetUserId();
            var account = await _repositoryAccounts.GetById(accountUpdate.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var accountType = await _repositoryAccountTypes.GetById(accountUpdate.AccountTypeId, userId);
            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositoryAccounts.Update(accountUpdate);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            var userId = _userService.GetUserId();
            var account = _repositoryAccounts.GetById(id,userId);
            if(account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = _userService.GetUserId();
            var account = _repositoryAccounts.GetById(id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositoryAccounts.Delete(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _repositoryAccounts.GetById(id, userId);
            if(account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var model = _mapper.Map<AccountCreationViewModel>(account);

            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
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

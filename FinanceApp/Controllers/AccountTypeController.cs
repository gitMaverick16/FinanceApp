using Dapper;
using FinanceApp.Models;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IRepositoryAccountTypes _repositoryAccountTypes;
        private readonly IUserService _userService;
        public AccountTypeController(IRepositoryAccountTypes repositoryAccountTypes,
            IUserService userService)
        {
            _repositoryAccountTypes = repositoryAccountTypes;
            _userService = userService;
        }
        public IActionResult Create()
        {
             return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _repositoryAccountTypes.Get(userId);
            return View(accountTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }
            accountType.UsuarId = _userService.GetUserId();

            var alreadyExists = await _repositoryAccountTypes.Exists(accountType.Name, accountType.Id);
            if (alreadyExists)
            {
                ModelState.AddModelError(nameof(accountType.Name), 
                    $"El nombre {accountType.Name} ya existe");
                return View(accountType);
            }
            await _repositoryAccountTypes.Create(accountType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyAccountExists(string name)
        {
            var userId = _userService.GetUserId();
            var alreadyExists = await _repositoryAccountTypes.Exists(name, userId);

            if (alreadyExists)
            {
                return Json($"El nombre {name} ya existe");
            }
            return Json(true);
        }
        
    }
}

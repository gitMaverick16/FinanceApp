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
        public AccountTypeController(IRepositoryAccountTypes repositoryAccountTypes)
        {
            _repositoryAccountTypes = repositoryAccountTypes;
        }
        public IActionResult Create()
        {
             return View();
        }

        [HttpPost]
        public IActionResult Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }
            accountType.UsuarId = 1;
            _repositoryAccountTypes.Create(accountType);
            return View();
        }
    }
}

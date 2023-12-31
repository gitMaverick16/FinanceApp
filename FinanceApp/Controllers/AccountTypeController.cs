﻿using Dapper;
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
            accountType.UserId = _userService.GetUserId();

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

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _repositoryAccountTypes.GetById(id, userId);
            if(accountType is null)
            {
                return RedirectToAction("NotFound");
            }
            return View(accountType);
        }

        [HttpPost]
        public async Task<ActionResult> Update(AccountType accountType)
        {
            var userId = _userService.GetUserId();
            var accountTypeExists = await _repositoryAccountTypes.Exists(accountType.Name, userId);
            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositoryAccountTypes.Update(accountType);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _repositoryAccountTypes.GetById(id,userId);
            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _repositoryAccountTypes.GetById(id, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositoryAccountTypes.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sort([FromBody] int[] ids)
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _repositoryAccountTypes.Get(userId);
            var idsAccountTypes = accountTypes.Select(x => x.Id);

            var idsAccountTypesNotBelongToUser = ids.Except(idsAccountTypes).ToList();

            if(idsAccountTypesNotBelongToUser.Count > 0)
            {
                return Forbid();
            }
            var sortedAccountTypes = ids.Select((value, index) => new AccountType()
            {
                Id = value,
                Order = index + 1,
            }).AsEnumerable();
            await _repositoryAccountTypes.Sort(sortedAccountTypes);
            return Ok();
        }
        
    }
}

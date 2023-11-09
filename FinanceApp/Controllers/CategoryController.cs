using FinanceApp.Models;
using FinanceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepositoryCategories _repositoryCategories;
        private readonly IUserService _userService;

        public CategoryController(IRepositoryCategories repositoryCategories,
            IUserService userService)
        {
            _repositoryCategories = repositoryCategories;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var categories = await _repositoryCategories.Get(userId);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = _userService.GetUserId();
            category.Id = userId;
            await _repositoryCategories.Create(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _repositoryCategories.GetById(id, userId);
            if(category is null)
            {
                return RedirectToAction("NptFound", "Home");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category categoryUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryUpdate);
            }
            var userId = _userService.GetUserId();
            var category = await _repositoryCategories.GetById(categoryUpdate.Id, userId);
            if (category is null)
            {
                return RedirectToAction("NptFound", "Home");
            }
            categoryUpdate.UserId = userId;
            await _repositoryCategories.Update(categoryUpdate);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _repositoryCategories.GetById(id, userId);
            if (category is null)
            {
                return RedirectToAction("NptFound", "Home");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _repositoryCategories.GetById(id, userId);
            if (category is null)
            {
                return RedirectToAction("NptFound", "Home");
            }
            await _repositoryCategories.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

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
    }
}

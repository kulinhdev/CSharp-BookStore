using BookStoreWeb.Models;
using BookStoreWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Category
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.CategoryRepository.GetAll();

            return View(objCategoryList);
        }


        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            //if (product.Name == product.Description)
            //{
            //    ModelState.AddModelError("name", "The name can't be same with description !");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Create category successfully!";

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/Id
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var findCategory = _unitOfWork.CategoryRepository.GetFirstOrDefault(m => m.Id == id);

            if (findCategory == null)
            {
                return NotFound();
            }


            return View(findCategory);
        }

        // POST: Category/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Update category successfully!";

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var findCategory = _unitOfWork.CategoryRepository.GetFirstOrDefault(m => m.Id == id);

            if (findCategory == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Remove(findCategory);
            _unitOfWork.Save();
            TempData["success"] = "Remove category successfully!";

            return RedirectToAction("Index");
        }
    }
}

using BookStoreWeb.Data;
using BookStoreWeb.Models;
using BookStoreWeb.Models.ViewModels;
using BookStoreWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.ProductRepository.GetAll();

            return View(objProductList);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (product.Name == product.Description)
            {
                ModelState.AddModelError("name", "Name can't be same with description !");
            }

            if (product.Price == product.SalePrice)
            {
                ModelState.AddModelError("salePrice", "Sale Price must be lower than Price !");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();
                TempData["success"] = "Create product successfully!";

                return RedirectToAction("Index");
            }


            return View(product);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var findProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(m => m.Id == id);

            if (findProduct == null)
            {
                return NotFound();
            }


            return View(findProduct);
        }

        [HttpPost("update")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product)
        {
            if (product.Price == product.SalePrice)
            {
                ModelState.AddModelError("salePrice", "Sale Price must be lower than Price !");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Update product successfully!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {

            // Get list categories
            ProductVM productVM = new()
            {
                Product = new(),
                selectListCategories = _unitOfWork.CategoryRepository.GetAll().Select(
                category =>
                new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                }),
                selectListAuthors = _unitOfWork.AuthorRepository.GetAll().Select(
                author =>
                new SelectListItem
                {
                    Text = author.Name,
                    Value = author.Id.ToString(),
                })
            };


            // Create case
            if (id == null || id == 0)
            {
                return View(productVM);
            }

            // Update case
            var findProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);

            if (findProduct == null)
            {
                return NotFound();
            }

            productVM.Product = findProduct;

            return View(productVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? imageFile)
        {
            // Validate Price
            if (productVM.Product.Price <= productVM.Product.SalePrice)
            {
                ModelState.AddModelError("Product.SalePrice", "Sale Price must be lower than Price !");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadsPath = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(imageFile.FileName);

                    // Handle remove old file
                    if (productVM.Product.ImagePath != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Handle upload new file 
                    using (var fileStreams = new FileStream(Path.Combine(uploadsPath, fileName + extension), FileMode.Create))
                    {
                        imageFile.CopyTo(fileStreams);
                    }
                    productVM.Product.ImagePath = @"\images\products\" + fileName + extension;

                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);

                    TempData["success"] = "Create product successfully!";
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);

                    TempData["success"] = "Update product successfully!";
                }

                //productVM.Product.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            // Get Categories and Authors
            productVM = new()
            {
                Product = productVM.Product,
                selectListCategories = _unitOfWork.CategoryRepository.GetAll().Select(
                category =>
                new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                }),
                selectListAuthors = _unitOfWork.AuthorRepository.GetAll().Select(
                author =>
                new SelectListItem
                {
                    Text = author.Name,
                    Value = author.Id.ToString(),
                })
            };

            return View(productVM);
        }

        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.ProductRepository.Remove(product);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Remove product successfully!";

        //    return RedirectToAction("Index");
        //}

        #region API ENDPOINT
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,Author");
            return Json(new { data = productList });
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int? id)
        {
            Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return Json(new { success = false, message = "Not found product with id: " + id });
            }

            // Handle remove old file
            if (product.ImagePath != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImagePath.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Remove product with id: " + id + " successfully!" });
        }

        #endregion
    }
}

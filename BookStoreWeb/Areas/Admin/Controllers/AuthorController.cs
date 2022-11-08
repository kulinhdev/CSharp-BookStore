using BookStoreWeb.Models;
using BookStoreWeb.Models.ViewModels;
using BookStoreWeb.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthorController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Author
        public IActionResult Index()
        {
            IEnumerable<Author> objAuthorList = _unitOfWork.AuthorRepository.GetAll();

            return View(objAuthorList);
        }


        // GET: Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author Author, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Upload file
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadsPath = Path.Combine(wwwRootPath, @"images\authors");
                    var extension = Path.GetExtension(imageFile.FileName);

                    // Handle upload new file 
                    using (var fileStreams = new FileStream(Path.Combine(uploadsPath, fileName + extension), FileMode.Create))
                    {
                        imageFile.CopyTo(fileStreams);
                    }

                    Author.AvatarUrl = @"\images\authors\" + fileName + extension;
                }

                // Set date format
                Author.Birthday.ToString("yyyy-MM-dd HH:mm:ss.fff");

                // Create Process
                _unitOfWork.AuthorRepository.Add(Author);
                _unitOfWork.Save();
                TempData["success"] = "Create Author successfully!";

                return RedirectToAction("Index");
            }

            return View(Author);
        }

        // GET: Author/Edit/Id
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var findAuthor = _unitOfWork.AuthorRepository.GetFirstOrDefault(m => m.Id == id);

            if (findAuthor == null)
            {
                return NotFound();
            }


            return View(findAuthor);
        }

        // POST: Author/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author Author, IFormFile? imageFile)
        {

            if (ModelState.IsValid)
            {
                // Upload file
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadsPath = Path.Combine(wwwRootPath, @"images\authors");
                    var extension = Path.GetExtension(imageFile.FileName);

                    // Handle remove old file
                    if (Author.AvatarUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, Author.AvatarUrl.TrimStart('\\'));
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

                    Author.AvatarUrl = @"\images\authors\" + fileName + extension;

                }

                // Set date format
                Author.Birthday.ToString("yyyy-MM-dd HH:mm:ss.fff");

                // Update Process
                _unitOfWork.AuthorRepository.Update(Author);
                _unitOfWork.Save();
                TempData["success"] = "Update Author successfully!";

                return RedirectToAction("Index");
            }

            return View(Author);
        }

        // GET: Author/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var findAuthor = _unitOfWork.AuthorRepository.GetFirstOrDefault(m => m.Id == id);

            if (findAuthor == null)
            {
                return NotFound();
            }

            _unitOfWork.AuthorRepository.Remove(findAuthor);
            _unitOfWork.Save();
            TempData["success"] = "Remove Author successfully!";

            return RedirectToAction("Index");
        }
    }
}

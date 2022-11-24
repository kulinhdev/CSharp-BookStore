using BookStoreWeb.Models;
using BookStoreWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookStoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Route("")]
        [Route("home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("products")]
        public IActionResult Product()
        {
            IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,Author");

            return View(productList);
        }

        [Route("products/details/{productSlug}")]
        public IActionResult Details(string productSlug)
        {
            Product productDetail = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Slug.Equals(productSlug), includeProperties: "Category,Author");

            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productDetail.Id,
                Product = productDetail
            };

            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToCart(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.UserId = claim.Value;

            // Check Cart exits
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(
                s => s.UserId == claim.Value && s.ProductId == shoppingCart.ProductId
                );

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.IncrementCount(shoppingCart, shoppingCart.Count);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Product));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
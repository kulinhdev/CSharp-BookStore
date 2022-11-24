using BookStoreWeb.Models;
using BookStoreWeb.Models.ViewModels;
using BookStoreWeb.Repository.IRepository;
using BookStoreWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")] 
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Get Current Login User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.UserId == claim.Value, 
                includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.SalePrice * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            // Get Current Login User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.UserId == claim.Value,
                includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.DisplayName;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.SalePrice * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.UserId == claim.Value,
                includeProperties: "Product");


            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.UserId = claim.Value;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusInProcess;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.SalePrice * cart.Count);
            }
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == claim.Value);

            
            _unitOfWork.OrderHeaderRepository.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            // Handle Order Detail
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Product.SalePrice,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetailRepository.Add(orderDetail);
                _unitOfWork.Save();
            }

            // Remove All Cart
            _unitOfWork.ShoppingCartRepository.RemoveList(ShoppingCartVM.ListCart);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cart);
                var count = _unitOfWork.ShoppingCartRepository.GetAll(u => u.UserId == cart.UserId).ToList().Count - 1;
                //HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCartRepository.GetAll(u => u.UserId == cart.UserId).ToList().Count;
            //HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }
    }
}

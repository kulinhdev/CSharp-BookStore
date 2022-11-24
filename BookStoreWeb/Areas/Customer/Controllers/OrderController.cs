using BookStoreWeb.Data;
using BookStoreWeb.Models.ViewModels;
using BookStoreWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public OrderController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {
            // Get Order for current user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var orderList = _unitOfWork.OrderHeaderRepository.GetAll(u => u.UserId == claim.Value, includeProperties: "ApplicationUser");

            return View(orderList);
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetailRepository.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
            };
            return View(OrderVM);
        }
    }
}

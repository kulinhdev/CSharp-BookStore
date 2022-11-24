using BookStoreWeb.Data;
using BookStoreWeb.Models;
using BookStoreWeb.Models.ViewModels;
using BookStoreWeb.Repository.IRepository;
using BookStoreWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View();
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

        [HttpPost]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDeliver()
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusDelivery);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated To Delivery Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateShipped()
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusShipped, SD.PaymentStatusApproved);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated To Shipped Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusCancelled, SD.PaymentStatusRejected);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated To Cancelled Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        #region API ENDPOINT
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var orderList = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser");

            switch (status)
            {
                case "delivery":
                    orderList = orderList.Where(u => u.OrderStatus == SD.StatusDelivery);
                    break;
                case "inprocess":
                    orderList = orderList.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderList = orderList.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                default:
                    break;
            }


            return Json(new { data = orderList });
        }

        #endregion
    }
}

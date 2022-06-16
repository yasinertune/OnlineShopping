using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model;
using System.Security.Claims;

namespace OnlineShopping.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            IEnumerable<Order> order;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            order = _unitOfWork.Order.GetAll(u => u.AppUserId == claim.Value);

            return View(order);
        }

        public IActionResult CancelOrder(int id)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);

            if (order.OrderStatus == "Ordered")
                order.OrderStatus = "Cancel";

            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model.ViewModels;


namespace OnlineShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var orderList = _unitOfWork.Order.GetAll(x=>x.OrderStatus != "Teslim Edildi");
            return View(orderList);
        }

        public IActionResult Details(int Id)
        {
            OrderVM = new OrderVM
            {
                Order = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == Id, includeProperties: "AppUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(d => d.OrderId == Id, includeProperties: "Product")
             };
            return View(OrderVM);

        }

        [HttpPost]
        public IActionResult Delivered(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == orderVM.Order.Id);

            orderProduct.OrderStatus = "Delivered";

            _unitOfWork.Order.Update(orderProduct);
            _unitOfWork.Save();

            return RedirectToAction("Details", "Order", new { Id = orderVM.Order.Id });
        }

        [HttpPost]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == orderVM.Order.Id);

            orderProduct.OrderStatus = "Cancel";

            _unitOfWork.Order.Update(orderProduct);
            _unitOfWork.Save();


            return RedirectToAction("Details", "Order", new { Id = orderVM.Order.Id });
        }

        [HttpPost]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == orderVM.Order.Id);

            orderProduct.Address = orderVM.Order.Address;
            orderProduct.PostalCode = orderVM.Order.PostalCode;
            orderProduct.CellPhone = orderVM.Order.CellPhone;
            orderProduct.Name = orderVM.Order.Name;

            _unitOfWork.Order.Update(orderProduct);
            _unitOfWork.Save();

            return RedirectToAction("Details", "Order", new { Id = orderVM.Order.Id });
        }


    }
}

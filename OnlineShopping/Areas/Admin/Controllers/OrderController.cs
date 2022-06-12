using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model.ViewModels;


namespace OnlineShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}

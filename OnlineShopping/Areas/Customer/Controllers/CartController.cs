using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model.ViewModels;
using System.Security.Claims;

namespace OnlineShopping.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartVM CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                Order = new()
            };

            foreach (var cart in CartVM.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVM.Order.OrderPrice += (cart.Price * cart.Count);
            }
            return View();
        }

        public IActionResult Order()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                Order = new()
            };

            CartVM.Order.AppUser = _unitOfWork.AppUser.GetFirstOrDefault(u=>u.Id == claim.Value);

            CartVM.Order.Name = CartVM.Order.AppUser.FullName;
            CartVM.Order.CellPhone = CartVM.Order.AppUser.CellPhone;
            CartVM.Order.Address = CartVM.Order.AppUser.Address;
            CartVM.Order.PostalCode = CartVM.Order.AppUser.PostalCode;

            foreach (var cart in CartVM.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVM.Order.OrderPrice += (cart.Price * cart.Count);
            }
            return View();

        }
    }
}

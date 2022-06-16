using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model;
using OnlineShopping.Model.ViewModels;
using System.Security.Claims;

namespace OnlineShopping.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
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
                CartVM.Order.OrderPrice += (cart.Price);
            }
            return View(CartVM);
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
            return View(CartVM);

        }

        [HttpPost]
        [ActionName("Order")]
        [ValidateAntiForgeryToken]
        public IActionResult OrderPost(CartVM cartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                Order = new()
            };
            AppUser appUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);
            CartVM.Order.AppUser = appUser;
            CartVM.Order.OrderDate = System.DateTime.Now;
            CartVM.Order.AppUserId = claim.Value;
            CartVM.Order.Name = cartVM.Order.Name;
            CartVM.Order.CellPhone = cartVM.Order.CellPhone;
            CartVM.Order.Address = cartVM.Order.Address;
            CartVM.Order.PostalCode = cartVM.Order.PostalCode;
            CartVM.Order.OrderStatus = "Ordered";

            foreach (var cart in CartVM.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVM.Order.OrderPrice += (cart.Price);
            }

            _unitOfWork.Order.Add(CartVM.Order);
            _unitOfWork.Save();

            foreach (var cart in CartVM.ListCart)
            {
                OrderDetails OrderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = CartVM.Order.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(OrderDetails);
                _unitOfWork.Save();
            }

            List<Cart> Carts = _unitOfWork.Cart.GetAll(u => u.AppUserId == CartVM.Order.AppUserId).ToList();

            _unitOfWork.Cart.RemoveRange(Carts);
            _unitOfWork.Save();

            var cartCount = _unitOfWork.Cart.GetAll(u => u.AppUserId == claim.Value).ToList().Count;
            HttpContext.Session.SetInt32("SessionCartCount", cartCount);

            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }


        public IActionResult Increase(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);

            cart.Count += 1;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult decrease(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);

            if (cart.Count > 1)
            {
                cart.Count -= 1;
            }
            else
            {
                _unitOfWork.Cart.Remove(cart);
                var cartCount = _unitOfWork.Cart.GetAll(u => u.AppUserId == cart.AppUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32("SessionCartCount", cartCount);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}

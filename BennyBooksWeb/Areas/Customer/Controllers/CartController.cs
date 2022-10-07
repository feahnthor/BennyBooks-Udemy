using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using BennyBooks.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BennyBooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // use Dependancy injection to access the rest of our Models
        private readonly IEmailSender _emailSender;
        public int OrderTotal { get; set; }
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }


        public IActionResult Index()
        {
            if (User.Identity is ClaimsIdentity claimsIdentity) // casting using pattern matching https://docs.microsoft.com/en-Us/dotnet/csharp/fundamentals/tutorials/safely-cast-using-pattern-matching-is-and-as-operators
            {
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // claim should not be null as we are authorizing above. Should get userId

                ShoppingCartVM = new ShoppingCartVM()
                {
                    CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value,
                    includeProperties: "Product")
                };

                foreach (ShoppingCart cart in ShoppingCartVM.CartList)
                {
                    cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                    ShoppingCartVM.CartTotal += cart.Price * cart.Count;
                }
            }
            

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public async Task<IActionResult> Increase(Guid cartId)
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Decrease(Guid cartId)
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);

            if (cart.Count <= 1) // delete product from cart
                _unitOfWork.ShoppingCart.Remove(cart);
            else
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index)); // After editing the database, take us back to our main cart page
        }
        public async Task<IActionResult> Remove(Guid cartId) // remove specific item
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
                return price;
            else
            {
                if (quantity <= 100)
                    return price50;
            }
            return price100;
        }
    }
}

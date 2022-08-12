using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BennyBooksWeb.Areas.Customer.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork; // Allows us to access all the product/model

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return View(productList); // right click function then hit "go to View"
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

    public IActionResult Details(Guid productId) // use productId as a variable, if we use id it will automatically be mapped to our shopping cart model
    {
        ShoppingCart cartObj = new()
        {
            Count = 1, //assign the values we want from our class
            ProductId = productId,
            Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType")
        };

        return View(cartObj); // still need to create a view model since product is not enough
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] // Makes sure only someone who is logged in can access this 
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity) User.Identity; // we need the userId of the logged in user. User id is stored with a url as the key
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // claim should not be null as we are authorizing above. Should get userId
        shoppingCart.ApplicationUserId = claim.Value;

        _unitOfWork.ShoppingCart.Add(shoppingCart);
        _unitOfWork.SaveAsync();

        return RedirectToAction(nameof(Index)); // Redirects the page to our index action above, nameof() makes it so we don't use strings.
                                          // If it was in another Controller use an overload method RedirectToAction("Index", "ControllerName")
    }
}
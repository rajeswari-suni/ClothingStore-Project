using Microsoft.AspNetCore.Mvc;

namespace ClothingStore_Project.Controllers
{
    public class LoginController : Controller
    {
        static List<string> cartProducts = new List<string>();
        static List<int> cartPrices = new List<int>();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Verify(string mobileNumber)
        {
            ViewBag.MobileNumber = mobileNumber;
            return RedirectToAction("RoleSelection");
        }

        public IActionResult RoleSelection()
        {
            return View();
        }
        public IActionResult Brands()
        {
            return View();
        }

        public IActionResult Products(string brand)
        {
            ViewBag.Brand = brand;
            return View();
        }
        public IActionResult ProductDetails()
        {
            return View();
        }
        public IActionResult Cart(string product, int price)
        {
            cartProducts.Add(product);
            cartPrices.Add(price);

            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;

            ViewBag.Total = cartPrices.Sum();

            return View();
        }
        public IActionResult OrderSuccess()
        {
            cartProducts.Clear();
            cartPrices.Clear();

            return View();
        }
    }
    }

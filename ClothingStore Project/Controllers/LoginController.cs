using Microsoft.AspNetCore.Mvc;

namespace ClothingStore_Project.Controllers
{
    public class LoginController : Controller
    {
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
        public IActionResult Cart(string product, int price)
        {
            ViewBag.Product = product;
            ViewBag.Price = price;

            return View();
        }
    }
    }

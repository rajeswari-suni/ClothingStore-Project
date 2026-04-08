using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ClothingStore_Project.Controllers
{
    public class LoginController : Controller
    {
        static List<string> cartProducts = new List<string>();
        static List<int> cartPrices = new List<int>();
        static List<string> cartSizes = new List<string>();
        static List<string> cartColors = new List<string>();


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
        public IActionResult Cart(string product, int price,string size,string color)
        {
            if (!string.IsNullOrEmpty(product))
            {
                cartProducts.Add(product);
                cartPrices.Add(price);
                cartSizes.Add(size);
                cartColors.Add(color);
            }
            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;

            int total = 0;
            foreach (var p in cartPrices)
            {
                total += p;
            }

            ViewBag.Total = total;
            if(cartProducts.Count==0)
            { 
                ViewBag.Total = 0; 
            }

            return View();
        }
        public IActionResult Remove(int index)
        {
            if (index >= 0 && index < cartProducts.Count)
            {
                cartProducts.RemoveAt(index);
                cartPrices.RemoveAt(index);
                cartSizes.RemoveAt(index);
                cartColors.RemoveAt(index);
            }
            return RedirectToAction("Cart");
        }
       
        public IActionResult Address()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Payment(string name, string mobile, string address, string city, string pincode)
        {
            ViewBag.Name = name;
            ViewBag.Mobile = mobile;
            ViewBag.Address = address;
            ViewBag.City = city;
            ViewBag.Pincode = pincode;

            int total = 0;
            foreach (var p in cartPrices)
            {
                total += p;
            }

            ViewBag.Total = total;

            return View();
        }
        public IActionResult OrderSuccess()
        {

            return View();
        }
    }
    }

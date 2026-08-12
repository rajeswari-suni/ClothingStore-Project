using ClothingStore_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore_Project.Controllers
{
    public class SellerController : Controller
    {
        private readonly StoreDbContext _context;

        public SellerController(StoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SellerRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SellerRegister(Seller seller)
        {
            _context.Sellers.Add(seller);
            _context.SaveChanges();

            return RedirectToAction("SellerList");
        }
        public IActionResult SellerList()
        {
            var sellers = _context.Sellers.ToList();

            return View(sellers);
        }
        public IActionResult DeleteSeller(int id)
        {
            var seller = _context.Sellers.Find(id);

            if (seller != null)
            {
                _context.Sellers.Remove(seller);
                _context.SaveChanges();
            }

            return RedirectToAction("SellerList");
        }

        public IActionResult EditSeller(int id)
        {
            var seller = _context.Sellers.Find(id);

            return View(seller);
        }

        [HttpPost]
        public IActionResult EditSeller(Seller seller)
        {
            _context.Sellers.Update(seller);
            _context.SaveChanges();

            return RedirectToAction("SellerList");
        }

        public IActionResult SellerDetails(int id)
        {
            var seller = _context.Sellers.Find(id);

            return View(seller);
        }
        public IActionResult AddProduct(string brand)
        {
            ViewBag.Brand = brand;
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("ProductList");
        }

        public IActionResult ProductList()
        {
            var products = _context.Products.ToList();

            return View(products);
        }
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            _context.Products.Update(product);

            _context.SaveChanges();

            return RedirectToAction("ProductList");
        }
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("ProductList");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            var data = _context.Products.Find(product.ProductId);

            if (data != null)
            {
                _context.Products.Remove(data);

                _context.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Seller")
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
        public IActionResult ViewOrders()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }
        public IActionResult ShipOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = "Shipped";
                _context.SaveChanges();
            }

            return RedirectToAction("ViewOrders");
        }
        public IActionResult DeliverOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = "Delivered";
                _context.SaveChanges();
            }

            return RedirectToAction("ViewOrders");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

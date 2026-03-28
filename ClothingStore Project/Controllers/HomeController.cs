using System.Diagnostics;
using ClothingStore_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreDbContext _context;
        public HomeController(ILogger<HomeController> logger, StoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ProductDetails(string product, int price)
        {
            ViewBag.Product = product;
            ViewBag.Price = price;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

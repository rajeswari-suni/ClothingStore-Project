using System.Diagnostics;
using ClothingStore_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreDbContext _context;
        static List<string> cartProducts = new List<string>();
        static List<string> cartPrices = new List<string>();
        static List<string> cartSizes = new List<string>();
        static List<string> cartColors = new List<string>();
        public HomeController(ILogger<HomeController> logger, StoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Products(string type)
        {
            ViewBag.Type = type;

            if (type == "tshirt")
            {
                ViewBag.Products = new List<string> { "Red T-Shirt", "Blue T-Shirt", "Black T-Shirt" };
            }
            else if (type == "innerwear")
            {
                ViewBag.Products = new List<string> { "Cotton Innerwear", "Sports Innerwear" };
            }
            else if (type == "shorts")
            {
                ViewBag.Products = new List<string> { "Black Shorts", "Gym Shorts" };
            }
            else if (type == "banyan")
            {
                ViewBag.Products = new List<string> { "White Banyan", "Black Banyan" };
            }

            return View();
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
        public IActionResult AddToCart(string product, int price, string size, string color, int qty)
        {
            for (int i = 0; i < qty; i++)
            {
                cartProducts.Add(product);
                cartPrices.Add(price.ToString());
                cartSizes.Add(size);
                cartColors.Add(color);
            }
            return RedirectToAction("Cart");
        }
        public IActionResult Cart()
        {
            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;

            ViewBag.Total = cartPrices.Sum(p => Convert.ToInt32(p));

            return View();
        }
        public IActionResult Remove(int index)
        {
            cartProducts.RemoveAt(index);
            cartPrices.RemoveAt(index);

            return RedirectToAction("Cart");
        }
        public IActionResult OrderSummary()
        {
            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;
            
            ViewBag.Total = cartPrices.Sum(p => Convert.ToInt32(p));

            ViewBag.DeliveryDate = DateTime.Now.AddDays(3).ToString("dd MMM yyyy");

            ViewBag.Status = "Order Placed";

            return View();
        }
        public IActionResult Address()
        {
            return View();
        }
        
        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult BuyNow(string product, int price, string size, string color, int qty)
        {
            cartProducts.Clear();
            cartPrices.Clear();
            cartSizes.Clear();
            cartColors.Clear();

            for (int i = 0; i < qty; i++)
            {
                cartProducts.Add(product);
                cartPrices.Add(price.ToString());
                cartSizes.Add(size);
                cartColors.Add(color);
            }
            ViewBag.Product = product;
            ViewBag.Price = price;
            ViewBag.Size = size;
            ViewBag.Color = color;
            ViewBag.Qty = qty;
            
            return View("OrderSummary");
        }
        [HttpPost]
        public IActionResult OrderSuccess(string name)
        {
            ViewBag.Name = name;
            ViewBag.Status = "Order Placed";

            // clear cart
            cartProducts.Clear();
            cartPrices.Clear();
            cartSizes.Clear();
            cartColors.Clear();

            return View();
        }
        public IActionResult AgentList()
        {
            var agents = _context.Agents.ToList();
            return View(agents);
        }
        public IActionResult DeleteAgent(int id)
        {
            var agent = _context.Agents.Find(id);

            if (agent != null)
            {
                _context.Agents.Remove(agent);
                _context.SaveChanges();
            }

            return RedirectToAction("AgentList");
        }
        public IActionResult EditAgent(int id)
        {
            var agent = _context.Agents.Find(id);

            return View(agent);
        }

        [HttpPost]
        public IActionResult EditAgent(Agent agent)
        {
            _context.Agents.Update(agent);
            _context.SaveChanges();

            return RedirectToAction("AgentList");
        }
        public IActionResult AgentDetails(int id)
        {
            var agent = _context.Agents.Find(id);

            return View(agent);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AgentRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgentRegister(Agent agent)
        {
            _context.Agents.Add(agent);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

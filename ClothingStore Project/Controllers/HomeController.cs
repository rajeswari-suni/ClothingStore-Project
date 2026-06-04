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
        static List<int> cartQtys = new List<int>();
        static string buyerName = "";
        public HomeController(ILogger<HomeController> logger, StoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        /* public IActionResult Products(string type)
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
         }*/
        public IActionResult Products(string type)
        {
            ViewBag.Type = type;

            var products = _context.Products.ToList();

            if (!string.IsNullOrEmpty(type))
            {
                products = products
                    .Where(x => x.ProductName.ToLower().Contains(type.ToLower()))
                    .ToList();
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Products");
            }

            return View(product);
        }
        public IActionResult AddToCart(string product, int price, string size, string color, int qty)
        {
          
                cartProducts.Add(product);
                cartPrices.Add(price.ToString());
                cartSizes.Add(size);
                cartColors.Add(color);
                cartQtys.Add(qty);
            
            return RedirectToAction("Cart");
        }
        public IActionResult Cart()
        {
            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;
            ViewBag.Qtys = cartQtys;


            int total = 0;

            for (int i = 0; i < Math.Min(cartPrices.Count, cartQtys.Count); i++)
            {
                total += Convert.ToInt32(cartPrices[i]) * cartQtys[i];
            }

            /* if (total >= 3000 && total < 5000)
             {
                 discount = total * 5 / 100;
             }
             else if (total >= 5000 && total < 10000)
             {
                 discount = total * 10 / 100;
             }
             else if (total >= 10000)
             {
                 discount = total * 15 / 100;
             }*/

            int discount = 0;

            string? role = HttpContext.Session.GetString("UserRole");

            if (role == "Agent")
            {
                int totalQty = cartQtys.Sum();

                if (totalQty >= 5)
                {
                    int agentDiscount =
                    HttpContext.Session.GetInt32("AgentDiscount") ?? 0;

                    discount = total * agentDiscount / 100;
                }
                else
                {
                    ViewBag.Message =
                    "Agents can purchase only bulk orders (Minimum 5 quantity)";
                }
            }
            else
            {
                ViewBag.Message = "";
            }

                ViewBag.Total = total;
                ViewBag.Discount = discount;
                ViewBag.FinalAmount = total - discount;

            return View();
        }
        public IActionResult Remove(int index)
        {
            cartProducts.RemoveAt(index);
            cartPrices.RemoveAt(index);

            return RedirectToAction("Cart");
        }
        public IActionResult IncreaseQty(int index)
        {
            if (index >= 0 && index < cartQtys.Count)
            {
                cartQtys[index]++;
            }

            return RedirectToAction("Cart");
        }

        public IActionResult DecreaseQty(int index)
        {
            if (index >= 0 && index < cartQtys.Count && cartQtys[index] > 1)
            {
                cartQtys[index]--;
            }

            return RedirectToAction("Cart");
        }
        public IActionResult OrderSummary()
        {
            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;
            ViewBag.Qtys = cartQtys;

            int total = 0;

            for(int i = 0; i< Math.Min(cartPrices.Count,cartQtys.Count);i++)
            {
                total += Convert.ToInt32(cartPrices[i]) * cartQtys[i];
            }

            /*int discount = 0;

            if (total >= 3000 && total < 5000)
            {
                discount = total * 5 / 100;
            }
            else if (total >= 5000 && total < 10000)
            {
                discount = total * 10 / 100;
            }
            else if (total >= 10000)
            {
                discount = total * 15 / 100;
            }*/

            int discount = 0;

            string role = HttpContext.Session.GetString("UserRole") ?? "";

            if (role == "Agent")
            {
                int agentDiscount =
                    HttpContext.Session.GetInt32("AgentDiscount") ?? 0;

                discount = total * agentDiscount / 100;
            }
            int finalAmount = total - discount;

            ViewBag.Total = total;
            ViewBag.Discount = discount;
            ViewBag.FinalAmount = finalAmount;

            ViewBag.Total = total;
            
            ViewBag.Debug = total;

            ViewBag.DeliveryDate = DateTime.Now.AddDays(3).ToString("dd MMM yyyy");

            ViewBag.Status = "Order placed";           

            return View();
        }
        public IActionResult Address()
        {
            return View();
        }

        /*public IActionResult Payment()
        {
            return View();
        }*/
        [HttpPost]
        public IActionResult Payment(Address address)
        {
            buyerName = address.Name;

            return View();
        }
        public IActionResult BuyNow(string product, int price, string size, string color, int qty)
        {
            
            Console.WriteLine($"Product={product},Price={price},Qty={qty}");
            cartProducts.Clear();
            cartPrices.Clear();
            cartSizes.Clear();
            cartColors.Clear();
            cartQtys.Clear();
    
            
            cartProducts.Add(product);
            cartPrices.Add(price.ToString());
            cartSizes.Add(size);
            cartColors.Add(color);
            cartQtys.Add(qty);

            Console.WriteLine(cartProducts.Count);
            Console.WriteLine(cartPrices.Count);
            Console.WriteLine(cartQtys.Count);

            ViewBag.TestProduct = product;
            ViewBag.TestPrice = price;
            ViewBag.TestQty = qty;

            ViewBag.Products = cartProducts;
            ViewBag.Prices = cartPrices;
            ViewBag.Sizes = cartSizes;
            ViewBag.Colors = cartColors;
            ViewBag.Qtys = cartQtys;

            ViewBag.Count1 = cartProducts.Count;
            ViewBag.Count2 = cartPrices.Count;
            ViewBag.Count3 = cartQtys.Count;

            //int total = price * qty;//

            int total = 0;

            for (int i = 0; i < Math.Min(cartPrices.Count, cartQtys.Count); i++)
            {
                total += Convert.ToInt32(cartPrices[i]) * cartQtys[i];
            }
            

            ViewBag.Total = total;
            ViewBag.Discount = 0;
            ViewBag.FinalAmount = total;

            return View("OrderSummary");
        }
        [HttpPost]
        public IActionResult OrderSuccess(string name)
        {
            if (cartProducts.Count > 0)
            {
                Order order = new Order();

                order.ProductName = cartProducts[0];
                order.Price = Convert.ToInt32(cartPrices[0]);
                order.Size = cartSizes[0];
                order.Color = cartColors[0];
                order.Quantity = cartQtys[0];
                order.BuyerName = buyerName;
                order.OrderDate = DateTime.Now;

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            ViewBag.Name = name;
            ViewBag.Status = "Order Placed";
            ViewBag.ProductName = cartProducts[0];

            // clear cart
            cartProducts.Clear();
            cartPrices.Clear();
            cartSizes.Clear();
            cartColors.Clear();
            cartQtys.Clear();

            return View();
        }
        [HttpPost]
        public IActionResult SaveRating(string productName, int stars)
        {
            Rating rating = new Rating();

            rating.ProductName = productName;
            rating.Stars = stars;

            _context.Ratings.Add(rating);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult AgentList()
        {
            var agents = _context.Agents.ToList();
            return View(agents);
        }
        public IActionResult OrdersList()
        {
            var orders = _context.Orders
                                 .OrderByDescending(x => x.OrderDate)
                                 .ToList();

            return View(orders);
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
        public IActionResult MyOrders()
        {
            var orders = _context.Orders.ToList();

            return View(orders);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult AgentRegister()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AgentRegister(Agent agent)
        {
            agent.AgentCode = "AG" + new Random().Next(1000, 9999);
            HttpContext.Session.SetInt32("AgentDiscount", agent.DiscountPercentage);

            _context.Agents.Add(agent);
            _context.SaveChanges();           

            return RedirectToAction("Products","Home");
        }
        
    }
}

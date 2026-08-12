using ClothingStore_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.IO.Compression;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Collections.Specialized.BitVector32;

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
                 public IActionResult Products(string type)
         {
             ViewBag.Type = type;

             var products = _context.Products.ToList();

            if (!string.IsNullOrWhiteSpace(type))
            {
                string search = type.ToLower()
                                    .Replace("-", "")
                                    .Replace(" ", "")
                                    .Trim();

                if (search.EndsWith("s"))
                {
                    search = search.Substring(0, search.Length - 1);
                }

                products = products.Where(x =>
                {
                    string product = x.ProductName.ToLower()
                                                  .Replace("-", "")
                                                  .Replace(" ", "")
                                                  .Trim();

                    if (product.EndsWith("s"))
                    {
                        product = product.Substring(0, product.Length - 1);
                    }

                    return product.Contains(search)
                        || search.Contains(product)
                        || product.StartsWith(search);
                }).ToList();
            }

            return View(products);
         }
      

        public IActionResult Privacy ()
        {
            return View();
        }
        public IActionResult Brands()
        {
            return View();
        }
              
        public IActionResult About ()
        {
            return View();
        }
        public IActionResult Contact ()
        {
            return View();
        }
        public IActionResult Terms ()
        {
            return View();
        }
        public IActionResult OrderList()
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

            ViewBag.AvgRating = _context.Ratings
                .Where(r => r.ProductName == product.ProductName)
                .Average(r => (double?)r.Stars) ?? 0;

            ViewBag.TotalReviews = _context.Ratings
                .Count(r => r.ProductName == product.ProductName);
            ViewBag.Reviews = _context.Ratings
    .Where(r => r.ProductName == product.ProductName)
    .OrderByDescending(r => r.Id)
    .ToList();

            return View(product);
        }
        public IActionResult Review(string productName)
        {
            ViewBag.ProductName = productName;
            return View();
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

        [HttpPost]
        public IActionResult Payment(Address address)
        {
            buyerName = address.Name??"";
            HttpContext.Session.SetString("DeliveryMobile", address.Mobile ?? "");
            AddressDetails data = new AddressDetails();

            data.UserMobile = HttpContext.Session.GetString("UserName") ?? "";
            data.Name = address.Name ?? "";
            data.Mobile = address.Mobile ?? "";
            data.DoorNumber = address.DoorNumber ?? "";
            data.AddressLine = address.AddressLine ?? "";
            data.City = address.City ?? "";
            data.State = address.State ?? "";
            data.Landmark = address.Landmark;
            data.Pincode = address.Pincode ?? "";

            _context.Addresses.Add(data);
            _context.SaveChanges();
            return View();
        }
        public IActionResult SavedAddresses()
        {
            string? mobile = HttpContext.Session.GetString("DeliveryMobile");

            var address = _context.Addresses
                .FirstOrDefault(a => a.UserMobile == mobile);

            if (address == null)
            {
                return RedirectToAction("Address");
            }

            return View(address);
        }
        public IActionResult EditAddress()
        {
            string? mobile = HttpContext.Session.GetString("DeliveryMobile");

            var address = _context.Addresses
                .FirstOrDefault(x => x.UserMobile == mobile);

            return View(address);
        }
        [HttpPost]
        public IActionResult EditAddress(AddressDetails model)
        {
            var address = _context.Addresses.Find(model.Id);

            if (address != null)
            {
                address.Name = model.Name;
                address.Mobile = model.Mobile;
                address.DoorNumber = model.DoorNumber;
                address.AddressLine = model.AddressLine;
                address.City = model.City;
                address.State = model.State;
                address.Landmark = model.Landmark;
                address.Pincode = model.Pincode;

                _context.SaveChanges();
            }

            return RedirectToAction("SavedAddresses");
        }
        public IActionResult DeleteAddress()
        {
            string? mobile = HttpContext.Session.GetString("DeliveryMobile");

            var address = _context.Addresses
                .FirstOrDefault(x => x.UserMobile == mobile);

            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
            }

            return RedirectToAction("Address");
        }
        public IActionResult BuyNow(string product, int price, string size, string color, int qty)
        {
            
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
                order.Status = "Placed";

                string? role = HttpContext.Session.GetString("UserRole");

                if (role == "Agent")
                {
                    order.CustomerType = "Agent";
                }
                else
                {
                    order.CustomerType = "Buyer";
                }
                order.UserMobile = HttpContext.Session.GetString("DeliveryMobile");

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
        public IActionResult SaveRating(string productName, int stars, string review)
        {
            Rating rating = new Rating();

            rating.ProductName = productName;
            rating.Stars = stars;

            rating.UserName = HttpContext.Session.GetString("UserName")??"Customer";

            rating.Review = review;

            _context.Ratings.Add(rating);
            _context.SaveChanges();

            return RedirectToAction("ProductDetails",
                new { id = Request.Form["productId"] });
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
        /* public IActionResult MyOrders()
         {
             var orders = _context.Orders.ToList();

             return View(orders);
         }*/
        public IActionResult MyOrders()
        {
            var mobile = HttpContext.Session.GetString("DeliveryMobile");

            var orders = _context.Orders
                .Where(o => o.UserMobile == mobile)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult WriteReview(string productName)
        {
            ViewBag.ProductName = productName;
            return View();
        }
        [HttpPost]
        public IActionResult WriteReview(string productName, int stars, string review)
        {
            var buyerName = _context.Orders
             .Where(o => o.ProductName == productName)
             .Select(o => o.BuyerName)
             .FirstOrDefault();

            var rating = new Rating
            {
                ProductName = productName,
                Stars = stars,
                UserName = buyerName,
                Review = review,
                ReviewDate=DateTime.Now
            };
            var order = _context.Orders
    .FirstOrDefault(o => o.ProductName == productName && o.BuyerName == buyerName);

            if (order != null)
            {
                order.IsReviewed = true;
            }

            _context.Ratings.Add(rating);
            _context.SaveChanges();

            return RedirectToAction("MyOrders");
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
        public IActionResult AgentRegister()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AgentRegister(Agent agent)
        {
            agent.AgentCode = "AG" + new Random().Next(1000, 9999);
            agent.SubscriptionStart = DateTime.Now;
            agent.SubscriptionEnd = DateTime.Now.AddMonths(1);
            agent.IsActive = true;

            HttpContext.Session.SetInt32("AgentDiscount", agent.DiscountPercentage);

            _context.Agents.Add(agent);
            _context.SaveChanges();           

            return RedirectToAction("Index","Home");
        }
        
    }
}

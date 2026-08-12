using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

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
        public IActionResult Verify(string mobileNumber, string role)
        {
            if (string.IsNullOrEmpty(role))
                role = "";

            if (string.IsNullOrEmpty(mobileNumber))
                mobileNumber = "";

            HttpContext.Session.SetString("Role", role);
            HttpContext.Session.SetString("Mobile", mobileNumber);

            return RedirectToAction("SendOTP",
                new { mobileNumber = mobileNumber, role = role });
        }


        [HttpGet]
        public async Task<IActionResult> SendOTP(string mobileNumber, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return Content("ROLL IS NULL");
            }
            if(string.IsNullOrEmpty(mobileNumber))
            {
                mobileNumber = "";
            }
                HttpContext.Session.SetString("Role", role);

             HttpContext.Session.SetString("Moblile", mobileNumber);

             HttpContext.Session.SetString("UserName", mobileNumber);

             string otp = new Random().Next(1000, 9999).ToString();
                HttpContext.Session.SetString("OTP", otp);
                string apiKey = "e01bP5qRNnhA42ZFws9KOdxpVmv6yJfiaGjBMCQSrLgXlTHD7EDQwCX0n7AJh9Ta2NPuiWc5zRkdrOEe";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("authorization", apiKey);

                    string url =
                    $"https://www.fast2sms.com/dev/bulkV2?authorization={apiKey}&route=q&message=Clothing Store Login Code {otp}&language=english&flash=0&numbers={mobileNumber}";

                    var response = await client.GetAsync(url);
                    //string result = await response.Content.ReadAsStringAsync();//
                    // return Content(result);//

                    if (response.IsSuccessStatusCode)
                    {
                      HttpContext.Session.SetString("OTP", otp);
                       ViewBag.MobileNumber = mobileNumber;
                    return View("Verify");
                    }
                   return Content("SMS Failed");
                   

            }
            
        }
        [HttpPost]
        public IActionResult VerifyOTP(string userOtp)
        {
            string? savedOtp = HttpContext.Session.GetString("OTP");
            string? role = HttpContext.Session.GetString("Role");

            if (userOtp == savedOtp)
            {

                if (role == "Seller")
                {
                    HttpContext.Session.SetString("UserRole", "Seller");
                    return RedirectToAction("Dashboard", "Seller");
                }
                else if (role == "Agent")
                {
                    HttpContext.Session.SetString("UserRole", "Agent");
                    return RedirectToAction("AgentRegister", "Home");
                }
                else if (role == "Buyer")
                {
                    HttpContext.Session.SetString("UserRole", "Buyer");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Brands", "Login");
                }
            }
            else
            {
                return Content("Invalid OTP");
            }
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

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
    }
}

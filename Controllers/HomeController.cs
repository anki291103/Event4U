using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event4U.Models;

namespace Event4U.Controllers
{
   public class HomeController : Controller
    {
        private readonly Event4UContext _context;

        public HomeController()
        {
            _context = new Event4UContext();
        }

        public ActionResult Index()
        {
            var offers = _context.Offers.OrderByDescending(o => o.CreatedAt).ToList();
            ViewBag.Offers = offers;
            return View();
        }
        public ActionResult UpcomingEvents()
{
    DateTime now = DateTime.Now;
    DateTime twoWeeksLater = now.AddDays(7);

    var upcomingEvents = _context.Bookings
        .Where(b => b.EventDate >= now && b.EventDate <= twoWeeksLater)
        .OrderBy(b => b.EventDate)
        .ToList();

    return View(upcomingEvents);
}






        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
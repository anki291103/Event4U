using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Event4U.Models; // Ensure this includes your context and view model namespaces
using Event4U.Models.ViewModel;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;

namespace Event4U.Controllers
{
    public class AdminController : Controller
    {
        private readonly Event4UContext _context;

        public AdminController()
        {
            _context = new Event4UContext();
        }

        // ✅ Admin Dashboard
        public ActionResult Index()
        {
            // 🔐 Check if logged in as admin
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                return RedirectToAction("Login", "User");

            // Gather dashboard stats
            var totalUsers = _context.Users.Count();
            var totalBookings = _context.Bookings.Count();

            var upcomingEvents = _context.Bookings
                .Where(b => b.EventDate >= DateTime.Now)
                .ToList();

            var allOffers = _context.Offers.ToList(); // 🔹 Include offers
            var TotalPayments = _context.Payments.Count();

            // Pass everything to ViewModel
            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalBookings = totalBookings,
                UpcomingEvents = upcomingEvents,
                AllOffers = allOffers // 🔹 New property to support offers PlayCard
            };

            return View(viewModel);
        }

        // ✅ View All Bookings

        // View all bookings
        public ActionResult Bookings(string eventNameFilter, DateTime? dateFilter)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                return RedirectToAction("Login", "User");

            var bookings = _context.Bookings
                .Include("User")
                .AsQueryable();

            if (!string.IsNullOrEmpty(eventNameFilter))
            {
                bookings = bookings.Where(b => b.EventName.Contains(eventNameFilter));
            }

            
            var viewModel = new AdminBookingViewModel
            {
                Bookings = bookings.ToList(),
                EventNameFilter = eventNameFilter,
            };

            return View(viewModel);
        }
        // Edit Booking (GET)
        [HttpGet]
        public ActionResult EditBooking(int id)
        {
            var booking = _context.Bookings
                .Include("User")
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return HttpNotFound();

            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", booking.UserId);
            return View(booking);
        }

        // Edit Booking (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(Booking updatedBooking)
        {
            if (ModelState.IsValid)
            {
                var booking = _context.Bookings.Find(updatedBooking.BookingId);
                if (booking == null)
                    return HttpNotFound();

                booking.UserId = updatedBooking.UserId;
                booking.FullName = updatedBooking.FullName;
                booking.Contact = updatedBooking.Contact;
                booking.Email = updatedBooking.Email;
                booking.EventName = updatedBooking.EventName;
                booking.EventDate = updatedBooking.EventDate;

                _context.SaveChanges();
                return RedirectToAction("Bookings");
            }

            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", updatedBooking.UserId);
            return View(updatedBooking);
        }

        // Delete Booking
        public ActionResult DeleteBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
            return RedirectToAction("Bookings");
        }


        // ✅ View All Users
        public ActionResult Users()
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                return RedirectToAction("Login", "User");

            var users = _context.Users.ToList();
            return View(users);
        }

        // AdminController.cs

        public ActionResult Offers()
        {
            var offers = _context.Offers.OrderByDescending(o => o.CreatedAt).ToList();
            return View(offers);
        }

        [HttpGet]
        public ActionResult AddOffer()
        {
            return View();
        }
        public ActionResult DeleteOffer(int id)
        {
            var offer = _context.Offers.Find(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
                _context.SaveChanges();
            }

            return RedirectToAction("AllOffers"); // 👈 redirect back to same page after deletion
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOffer(Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.CreatedAt = DateTime.Now;
                _context.Offers.Add(offer);
                _context.SaveChanges();
                return RedirectToAction("Offers");
            }
            return View(offer);
        }
        public ActionResult AllOffers()
        {
            var offers = _context.Offers.ToList(); // or wherever you're fetching from
            return View(offers);
        }
        public ActionResult AddUser()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "User added successfully!";
                return RedirectToAction("Users");
            }

            return View(user);
        }

        public ActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View("AddUser", user); // Reuse AddUser view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser == null)
                    return HttpNotFound();

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Users");
            }

            return View("AddUser", user);
        }

        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Users");
        }
        public ActionResult ManagePayments()
        {
            var payments = _context.Payments
                .Include("Booking")
                .ToList();

            return View(payments);
        }
        public ActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                BookingsPerMonth = GetBookingsPerMonth(),
                PopularEvents = GetPopularEventsByBookingCount(),
                RevenuePerEvent = GetEventRevenue()
            };

            return View(model);
        }

        private List<BookingStats> GetBookingsPerMonth()
        {
            var rawStats = _context.Bookings
                .GroupBy(b => b.EventDate.Month)
                .Select(g => new
                {
                    MonthNumber = g.Key,
                    Count = g.Count()
                })
                .ToList(); // Materialize the query here

            // Now apply .NET method safely in-memory
            var stats = rawStats
                .Select(x => new BookingStats
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.MonthNumber),
                    Count = x.Count
                })
                .ToList();

            return stats;
        }


        private List<PopularEventStats> GetPopularEventsByBookingCount()
        {
            var stats = _context.Bookings
                .GroupBy(b => b.EventName)
                .Select(g => new PopularEventStats
                {
                    EventName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return stats;
        }

        private List<RevenueStats> GetEventRevenue()
        {
            var stats = (from booking in _context.Bookings
                         join payment in _context.Payments
                         on booking.BookingId equals payment.BookingId
                         group payment by booking.EventName into g
                         select new RevenueStats
                         {
                             EventName = g.Key,
                             Total = g.Sum(x => x.TotalAmount)
                         })
                         .OrderByDescending(x => x.Total)
                         .ToList();

            return stats;
        }
        public ActionResult DownloadInvoice(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return HttpNotFound();
            }

            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var doc = new Document(pdf);

                // Header
                doc.Add(new Paragraph("🧾 Event4U Invoice")
                    .SetFontSize(20)
                    .SetTextAlignment(TextAlignment.CENTER));

                doc.Add(new Paragraph("\n"));

                // Invoice Details
                doc.Add(new Paragraph($"Invoice Date: {payment.PaidAt:dd-MMM-yyyy}"));
                doc.Add(new Paragraph($"Payment ID: {payment.PaymentId}"));
                doc.Add(new Paragraph($"Booking ID: {payment.BookingId}"));
                doc.Add(new Paragraph($"People Count: {payment.PeopleCount}"));
                doc.Add(new Paragraph($"Total Paid: ₹{payment.TotalAmount}"));

                doc.Close();

                return File(ms.ToArray(), "application/pdf", $"Invoice_{payment.PaymentId}.pdf");
            }
        }
    }

}



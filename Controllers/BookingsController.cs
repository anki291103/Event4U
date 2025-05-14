using System;
using System.Linq;
using System.Web.Mvc;
using Event4U.Models;

public class BookingsController : Controller
{
    private readonly Event4UContext _context;

    // ✅ Parameterless constructor added for compatibility
    public BookingsController()
    {
        _context = new Event4UContext();
    }

    // GET: Book
    public ActionResult Book()
    {
        return View();
    }

    // POST: Book


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Book(Booking booking)
    {
        if (ModelState.IsValid)
        {
            var email = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            booking.UserId = user.UserId;
            booking.EventDate = booking.EventDate.Date;

            // 🔒 Check if any booking exists on the same date
            bool isDateTaken = _context.Bookings.Any(b => b.EventDate == booking.EventDate);

            if (isDateTaken)
            {
                ModelState.AddModelError("EventDate", "An event is already scheduled on this date. Please select another day.");
                return View(booking); // Return back with error
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            TempData["Success"] = "Your booking was successful!";
            return RedirectToAction("Payment", "Payment", new { bookingId = booking.BookingId });
        }

        return View(booking);
    }

    public ActionResult BookingConfirmation()
    {
        return View();
    }

    public ActionResult Events()
    {
        return View();
    }
    [Authorize]
    public ActionResult BookEvent()
    {
        return View();
    }
    [Authorize]
    public ActionResult MyBookings()
    {
        var email = User.Identity.Name; // Gets the logged-in user's email

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account");
        }

        // Get the user from Users table using email
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch bookings that match the logged-in user's ID
        var bookings = _context.Bookings
                               .Where(b => b.UserId == user.UserId)
                               .ToList();

        return View(bookings);
    }
}


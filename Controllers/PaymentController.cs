// Required for .Include
using System;
using System.Linq;
using Event4U.Models;
using Event4U.Models.ViewModel;
using System.Web.Mvc;
using System.Data.Entity; // for EF6 (classic ASP.NET MVC)


public class PaymentController : Controller
{
    private readonly Event4UContext _context;

    public PaymentController()
    {
        _context = new Event4UContext(); // Manually instantiate
    }


    public ActionResult Payment(int bookingId)
    {
        var booking = _context.Bookings.Find(bookingId);
        if (booking == null)
        {
            return HttpNotFound();
        }

        var viewModel = new PaymentViewModel
        {
            BookingId = booking.BookingId,
            FullName = booking.FullName,
            EventName = booking.EventName,
            EventDate = booking.EventDate
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Payment(PaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var booking = _context.Bookings.Find(model.BookingId);
            if (booking == null)
                return HttpNotFound();

            decimal rate = 0;
            switch (booking.EventName.ToLower())
            {
                case "corporate":
                    rate = 20000;
                    break;
                case "wedding":
                    rate = 1000000;
                    break;
                case "party":
                    rate = 10000;
                    break;
                case "concert":
                    rate = 300000;
                    break;
                default:
                    rate = 5000; // default case (optional)
                    break;
            }

            var totalAmount = rate ;

            var payment = new Payment
            {
                BookingId = model.BookingId,
                PeopleCount = model.PeopleCount,
                TotalAmount = totalAmount, // ✅ NEW
                PaidAt = DateTime.Now
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return RedirectToAction("Confirmation", new { id = payment.PaymentId });
        }

        return View(model);
    }
    public ActionResult Confirmation(int id)
    {
        var payment = _context.Payments.Include(p => p.Booking).FirstOrDefault(p => p.PaymentId == id);
        if (payment == null)
            return HttpNotFound();

        var model = new PaymentConfirmationViewModel
        {
            FullName = payment.Booking.FullName,
            EventName = payment.Booking.EventName,
            EventDate = payment.Booking.EventDate,
            PeopleCount = payment.PeopleCount,
            TotalAmount = payment.TotalAmount
        };

        return View(model);
    }




}

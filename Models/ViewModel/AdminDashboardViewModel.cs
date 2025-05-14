using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event4U.Models; // Make sure this is included to access Booking & User

namespace Event4U.Models.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public List<Booking> UpcomingEvents { get; set; }
        public List<Offer> AllOffers { get; set; }
        public int TotalPayments { get; set; }

        // ✅ ADD THIS: For displaying user list
        public IEnumerable<BookingStats> BookingsPerMonth { get; set; }
        public IEnumerable<PopularEventStats> PopularEvents { get; set; }
        public IEnumerable<RevenueStats> RevenuePerEvent { get; set; }
    }

    public class BookingStats
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class PopularEventStats
    {
        public string EventName { get; set; }
        public int Count { get; set; }
    }

    public class RevenueStats
    {
        public string EventName { get; set; }
        public decimal Total { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event4U.Models.ViewModel
{
    public class AdminBookingViewModel
    {
        public List<Booking> Bookings { get; set; }

        // Filter inputs
        public string EventNameFilter { get; set; }
    }
}
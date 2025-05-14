using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event4U.Models
{
 
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int PeopleCount { get; set; }
        public decimal TotalAmount { get; set; } // ✅ NEW
        public DateTime PaidAt { get; set; }
    }

}
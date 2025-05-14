using System;

namespace Event4U.Models.ViewModel
{
    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        public string FullName { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public int PeopleCount { get; set; }
    }
}

// Models/ViewModel/BookingViewModel.cs
using System;

namespace Event4U.Models.ViewModel
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string UserName { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
    }
}

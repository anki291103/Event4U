using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event4U.Models.ViewModel
{
    public class PaymentConfirmationViewModel
    {
        public string FullName { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public int PeopleCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
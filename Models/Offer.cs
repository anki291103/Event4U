// Models/Offer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Event4U.Models
{
    public class Offer
    {
        public int OfferId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

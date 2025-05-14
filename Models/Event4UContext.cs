using System.Collections.Generic;
using System.Data.Entity;

namespace Event4U.Models
{
    public class Event4UContext : DbContext
    {
        public Event4UContext() : base("Event4UDB") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event4U.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        // ✅ Computed property for compatibility
        public string FullName => Name;
    }
}
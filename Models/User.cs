using CLDV1.Models.CLDV1.Models;

namespace CLDV1.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Order> Orders { get; set; } // Navigation property
        public List<Product> Products { get; set; } // Navigation property
    }
}

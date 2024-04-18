using CLDV1.Models.CLDV1.Models;

namespace CLDV1.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
        public int Quantity { get; set; }

        // Additional properties or constructors if needed
    }

}


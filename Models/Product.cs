namespace CLDV1.Models
{
    namespace CLDV1.Models
    {
        public class Product
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImagePath { get; set; }
            public string Category { get; set; }

            // Navigation properties
            public int UserId { get; set; }
            public User User { get; set; }
            public List<OrderItem> OrderItems { get; set; }
        }
    }
}

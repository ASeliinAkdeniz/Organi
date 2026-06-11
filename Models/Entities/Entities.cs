namespace Organi.Models.Entities
{
    public enum ProductCategory
    {
        Sebzeler = 1,
        Meyveler = 2,
        SutUrunleri = 3,
        Balikci = 4,
        Atistirmalik = 5,
        Icecekler = 6
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }
        public ProductCategory Category { get; set; }
        public string ImageUrl { get; set; } = "/images/placeholder.png";
        public double Rating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public string Unit { get; set; } = "kg";
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public string? Badge { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Besin değerleri
        public int? CaloriesPer100g { get; set; }
        public double? FatPer100g { get; set; }
        public double? CarbsPer100g { get; set; }
        public double? ProteinPer100g { get; set; }
        public string? ServingSize { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public double Quantity { get; set; } = 1;
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }

    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal ShippingFee { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public enum OrderStatus { Pending, Confirmed, Shipped, Delivered, Cancelled }

    public class ContactMessage
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = "Admin";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

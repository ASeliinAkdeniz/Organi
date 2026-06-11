using Organi.Models.Entities;

namespace Organi.Models.ViewModels
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; } = new();
        public ProductCategory? SelectedCategory { get; set; }
        public string? SearchTerm { get; set; }
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = 1000;
        public string? SortBy { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public List<Product> PopularProducts { get; set; } = new();
    }

    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.Product!.Price * (decimal)i.Quantity);
        public decimal ShippingFee => Subtotal >= 99 ? 0 : 9.99m;
        public decimal Total => Subtotal + ShippingFee;
    }

    public class CompareViewModel
    {
        public List<Product> Products { get; set; } = new();
    }

    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; } = new();
        public List<Product> NewProducts { get; set; } = new();
    }

    public class ProductCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }
        public ProductCategory Category { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Unit { get; set; } = "kg";
        public bool IsFeatured { get; set; }
        public string? Badge { get; set; }
        public int? CaloriesPer100g { get; set; }
        public double? FatPer100g { get; set; }
        public double? CarbsPer100g { get; set; }
        public double? ProteinPer100g { get; set; }
        public string? ServingSize { get; set; }
    }

    public class ProductUpdateViewModel : ProductCreateViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}

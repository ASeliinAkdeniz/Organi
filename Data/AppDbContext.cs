using Microsoft.EntityFrameworkCore;
using Organi.Models.Entities;

namespace Organi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().Property(p => p.OldPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.ShippingFee).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(o => o.UnitPrice).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>().HasData(
                new Product { Id=1, Name="Organik Biber", Price=2.29m, OldPrice=3.49m, Stock=50, Category=ProductCategory.Sebzeler, Unit="kg", Rating=4.5, RatingCount=15, Badge="İNDİRİM", Description="Çiftliğimizden taze organik biber.", CaloriesPer100g=31, FatPer100g=0.3, CarbsPer100g=6.0, ProteinPer100g=1.0, ServingSize="100g" },
                new Product { Id=2, Name="Organik Bitter Çikolata", Price=4.49m, Stock=30, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.5, RatingCount=14, Description="Doğal bitter çikolata.", CaloriesPer100g=550, FatPer100g=35.0, CarbsPer100g=45.0, ProteinPer100g=5.0, ServingSize="30g" },
                new Product { Id=3, Name="Organik Bulgur", Price=2.49m, Stock=100, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.0, RatingCount=8, Description="Tam buğday bulguru.", CaloriesPer100g=342, FatPer100g=1.3, CarbsPer100g=71.0, ProteinPer100g=12.0, ServingSize="50g" },
                new Product { Id=4, Name="Organik Ceviz", Price=9.99m, Stock=45, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.9, RatingCount=91, Badge="HOT", Description="Organik iç ceviz 200g.", CaloriesPer100g=654, FatPer100g=65.2, CarbsPer100g=14.0, ProteinPer100g=15.2, ServingSize="30g porsiyon" },
                new Product { Id=5, Name="Organik Çilek", Price=4.59m, OldPrice=5.99m, Stock=25, Category=ProductCategory.Meyveler, Unit="paket", Rating=4.5, RatingCount=30, Badge="İNDİRİM", Description="Taze organik çilek.", CaloriesPer100g=32, FatPer100g=0.3, CarbsPer100g=7.7, ProteinPer100g=0.7, ServingSize="100g" },
                new Product { Id=6, Name="Organik Elma", Price=2.99m, Stock=80, Category=ProductCategory.Meyveler, Unit="kg", Rating=4.5, RatingCount=22, Description="Doğal organik elma.", CaloriesPer100g=52, FatPer100g=0.2, CarbsPer100g=14.0, ProteinPer100g=0.3, ServingSize="1 adet (150g)" },
                new Product { Id=7, Name="Organik Fasulye", Price=2.29m, Stock=60, Category=ProductCategory.Sebzeler, Unit="kg", Rating=3.5, RatingCount=12, Description="Taze organik yeşil fasulye.", CaloriesPer100g=31, FatPer100g=0.2, CarbsPer100g=7.0, ProteinPer100g=1.8, ServingSize="100g" },
                new Product { Id=8, Name="Organik Fındık Karışımı", Price=8.99m, Stock=40, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.5, RatingCount=18, Description="Doğal fındık karışımı.", CaloriesPer100g=607, FatPer100g=54.0, CarbsPer100g=16.0, ProteinPer100g=15.0, ServingSize="30g" },
                new Product { Id=9, Name="Organik Granola Bar", Price=3.99m, Stock=100, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.4, RatingCount=27, Description="Organik doğal granola bar 40g.", CaloriesPer100g=430, FatPer100g=15.0, CarbsPer100g=57.0, ProteinPer100g=8.4, ServingSize="1 bar (45g)" },
                new Product { Id=10, Name="Organik Havuç", Price=1.49m, OldPrice=2.19m, Stock=70, Category=ProductCategory.Sebzeler, Unit="kg", Rating=4.0, RatingCount=19, Description="Taze organik havuç.", CaloriesPer100g=41, FatPer100g=0.2, CarbsPer100g=10.0, ProteinPer100g=0.9, ServingSize="100g" },
                new Product { Id=11, Name="Organik Kuru Kayısı", Price=5.49m, Stock=55, Category=ProductCategory.Atistirmalik, Unit="paket", Rating=4.5, RatingCount=33, Description="Doğal kuru kayısı.", CaloriesPer100g=241, FatPer100g=0.5, CarbsPer100g=63.0, ProteinPer100g=3.4, ServingSize="30g" },
                new Product { Id=12, Name="Organik Süt", Price=1.99m, Stock=90, Category=ProductCategory.SutUrunleri, Unit="litre", Rating=4.8, RatingCount=45, IsFeatured=true, Description="Taze organik süt 1 litre.", CaloriesPer100g=61, FatPer100g=3.2, CarbsPer100g=4.8, ProteinPer100g=3.2, ServingSize="200ml" }
            );
        }
    }
}

// ================================================================
// 1. OBSERVER PATTERN - Ürün değişikliklerini dinleyicilere iletir
// ================================================================
namespace Organi.Patterns.Observer
{
    public class ProductChangedEvent
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = "Admin";
        public DateTime OccurredAt { get; set; } = DateTime.Now;
        public Dictionary<string, string> ChangedFields { get; set; } = new();
    }

    public interface IProductObserver { Task OnProductChangedAsync(ProductChangedEvent evt); }

    public interface IProductEventPublisher
    {
        void Subscribe(IProductObserver observer);
        Task PublishAsync(ProductChangedEvent evt);
    }

    public class ProductEventPublisher : IProductEventPublisher
    {
        private readonly List<IProductObserver> _observers = new();
        public void Subscribe(IProductObserver observer) { if (!_observers.Contains(observer)) _observers.Add(observer); }
        public async Task PublishAsync(ProductChangedEvent evt) { foreach (var o in _observers) await o.OnProductChangedAsync(evt); }
    }

    public class AdminNotification { public string Message { get; set; } = string.Empty; public DateTime OccurredAt { get; set; } public bool IsRead { get; set; } }

    public class AdminNotificationObserver : IProductObserver
    {
        private static readonly List<AdminNotification> _notifications = new();
        public Task OnProductChangedAsync(ProductChangedEvent evt)
        {
            _notifications.Insert(0, new AdminNotification { Message = $"{evt.PerformedBy}, \"{evt.ProductName}\" ürününü {Tr(evt.Action)}.", OccurredAt = evt.OccurredAt });
            if (_notifications.Count > 50) _notifications.RemoveAt(_notifications.Count - 1);
            return Task.CompletedTask;
        }
        public static IReadOnlyList<AdminNotification> GetAll() => _notifications.AsReadOnly();
        public static void MarkAllRead() => _notifications.ForEach(n => n.IsRead = true);
        private static string Tr(string a) => a switch { "Created" => "ekledi", "Updated" => "güncelledi", "Deleted" => "sildi", _ => a };
    }

    public class AuditLogObserver : IProductObserver
    {
        private readonly string _logPath;
        public AuditLogObserver(IWebHostEnvironment env) { _logPath = Path.Combine(env.ContentRootPath, "Logs", "audit.log"); Directory.CreateDirectory(Path.GetDirectoryName(_logPath)!); }
        public async Task OnProductChangedAsync(ProductChangedEvent evt)
        {
            var line = $"[{evt.OccurredAt:yyyy-MM-dd HH:mm:ss}] {evt.Action.ToUpper()} | {evt.ProductName} (#{evt.ProductId}) | By: {evt.PerformedBy}";
            if (evt.ChangedFields.Any()) line += " | " + string.Join(", ", evt.ChangedFields.Select(kv => $"{kv.Key}={kv.Value}"));
            await File.AppendAllTextAsync(_logPath, line + Environment.NewLine);
        }
    }
}

// ================================================================
// 2. CHAIN OF RESPONSIBILITY - Ürün validasyon zinciri
// ================================================================
namespace Organi.Patterns.ChainOfResponsibility
{
    using Organi.Models.ViewModels;

    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public static ValidationResult Success() => new() { IsValid = true };
        public static ValidationResult Fail(string msg) => new() { IsValid = false, ErrorMessage = msg };
    }

    public abstract class ProductValidationHandler
    {
        protected ProductValidationHandler? _next;
        public ProductValidationHandler SetNext(ProductValidationHandler next) { _next = next; return next; }
        public abstract ValidationResult Handle(ProductCreateViewModel vm);
        protected ValidationResult PassToNext(ProductCreateViewModel vm) => _next?.Handle(vm) ?? ValidationResult.Success();
    }

    public class NameValidationHandler : ProductValidationHandler
    {
        public override ValidationResult Handle(ProductCreateViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name)) return ValidationResult.Fail("Ürün adı boş bırakılamaz.");
            if (vm.Name.Length < 2) return ValidationResult.Fail("Ürün adı en az 2 karakter olmalıdır.");
            return PassToNext(vm);
        }
    }

    public class PriceValidationHandler : ProductValidationHandler
    {
        public override ValidationResult Handle(ProductCreateViewModel vm)
        {
            if (vm.Price <= 0) return ValidationResult.Fail("Fiyat sıfırdan büyük olmalıdır.");
            return PassToNext(vm);
        }
    }

    public class StockValidationHandler : ProductValidationHandler
    {
        public override ValidationResult Handle(ProductCreateViewModel vm)
        {
            if (vm.Stock < 0) return ValidationResult.Fail("Stok negatif olamaz.");
            return PassToNext(vm);
        }
    }

    public class ImageValidationHandler : ProductValidationHandler
    {
        public override ValidationResult Handle(ProductCreateViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.ImageUrl)) vm.ImageUrl = "/images/placeholder.png";
            return PassToNext(vm);
        }
    }

    public static class ValidationChainBuilder
    {
        public static ProductValidationHandler Build()
        {
            var name = new NameValidationHandler();
            var price = new PriceValidationHandler();
            var stock = new StockValidationHandler();
            var image = new ImageValidationHandler();
            name.SetNext(price).SetNext(stock).SetNext(image);
            return name;
        }
    }
}

// ================================================================
// 3. FACTORY METHOD - Ürün nesnesi üretimi
// ================================================================
namespace Organi.Patterns.Factory
{
    using Organi.Models.Entities;
    using Organi.Models.ViewModels;

    public abstract class ProductFactory
    {
        public abstract Product Create(ProductCreateViewModel vm);
        protected void FillBase(Product p, ProductCreateViewModel vm)
        {
            p.Name = vm.Name.Trim();
            p.Description = vm.Description;
            p.Price = vm.Price;
            p.OldPrice = vm.OldPrice;
            p.Stock = vm.Stock;
            p.Category = vm.Category;
            p.ImageUrl = vm.ImageUrl;
            p.Unit = vm.Unit;
            p.IsFeatured = vm.IsFeatured;
            p.Badge = vm.Badge;
            p.CaloriesPer100g = vm.CaloriesPer100g;
            p.FatPer100g = vm.FatPer100g;
            p.CarbsPer100g = vm.CarbsPer100g;
            p.ProteinPer100g = vm.ProteinPer100g;
            p.ServingSize = vm.ServingSize;
            p.IsActive = true;
            p.CreatedAt = DateTime.Now;
        }
    }

    public class FreshProductFactory : ProductFactory
    {
        public override Product Create(ProductCreateViewModel vm) { var p = new Product(); FillBase(p, vm); return p; }
    }

    public static class ProductFactoryResolver
    {
        public static ProductFactory Resolve() => new FreshProductFactory();
    }
}

// ================================================================
// 4. STRATEGY - Kargo ücreti hesaplama stratejisi
// ================================================================
namespace Organi.Patterns.Strategy
{
    public interface IShippingStrategy { decimal Calculate(decimal subtotal); string GetLabel(); }

    public class FreeShippingStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal subtotal) => 0m;
        public string GetLabel() => "Ücretsiz Kargo";
    }

    public class StandardShippingStrategy : IShippingStrategy
    {
        public decimal Calculate(decimal subtotal) => 9.99m;
        public string GetLabel() => "Standart Kargo";
    }

    public class ShippingCalculator
    {
        private IShippingStrategy _strategy;
        public ShippingCalculator(decimal subtotal)
        {
            _strategy = subtotal >= 99m ? new FreeShippingStrategy() : new StandardShippingStrategy();
        }
        public decimal Calculate(decimal subtotal) => _strategy.Calculate(subtotal);
        public string GetLabel() => _strategy.GetLabel();
    }
}

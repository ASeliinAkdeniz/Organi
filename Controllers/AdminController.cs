using Microsoft.AspNetCore.Mvc;
using Organi.Data.UnitOfWork;
using Organi.Models.Entities;
using Organi.Models.ViewModels;
using Organi.Patterns.ChainOfResponsibility;
using Organi.Patterns.Factory;
using Organi.Patterns.Observer;

namespace Organi.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductEventPublisher _publisher;
        private readonly IWebHostEnvironment _env;

        public AdminController(IUnitOfWork uow, IProductEventPublisher publisher, IWebHostEnvironment env)
        {
            _uow = uow; _publisher = publisher; _env = env;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            ViewData["Title"] = "Dashboard";
            var products = await _uow.Products.GetAllAsync();
            var orders = await _uow.Orders.GetAllAsync();
            var messages = await _uow.ContactMessages.GetAllAsync();
            var logs = await _uow.AuditLogs.GetAllAsync();
            ViewBag.UnreadCount = messages.Count(m => !m.IsRead);

            var vm = new DashboardViewModel
            {
                TotalProducts = products.Count(),
                TotalOrders = orders.Count(),
                UnreadMessages = messages.Count(m => !m.IsRead),
                LowStockProducts = products.Count(p => p.Stock < 10),
                RecentOrders = orders.OrderByDescending(o => o.CreatedAt).Take(5).ToList(),
                RecentMessages = messages.OrderByDescending(m => m.CreatedAt).Take(5).ToList(),
                RecentLogs = logs.OrderByDescending(l => l.CreatedAt).Take(8).ToList()
            };
            return View(vm);
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Products";
            ViewData["Title"] = "Ürünler";
            var products = await _uow.Products.GetAllAsync();
            ViewBag.UnreadCount = (await _uow.ContactMessages.GetAllAsync()).Count(m => !m.IsRead);
            return View(products.OrderByDescending(p => p.CreatedAt).ToList());
        }

        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "Products";
            ViewData["Title"] = "Yeni Ürün";
            return View(new ProductCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel vm)
        {
            var chain = ValidationChainBuilder.Build();
            var result = chain.Handle(vm);
            if (!result.IsValid) { TempData["Error"] = result.ErrorMessage; return View(vm); }

            var factory = ProductFactoryResolver.Resolve();
            var product = factory.Create(vm);
            await _uow.Products.AddAsync(product);
            await _uow.SaveChangesAsync();

            await _uow.AuditLogs.AddAsync(new Organi.Models.Entities.AuditLog { Action = "Created", Description = $"\"{product.Name}\" ürünü eklendi.", PerformedBy = "Admin" });
            await _uow.SaveChangesAsync();

            await _publisher.PublishAsync(new ProductChangedEvent { ProductId = product.Id, ProductName = product.Name, Action = "Created" });
            TempData["Success"] = $"\"{product.Name}\" eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActiveMenu"] = "Products";
            ViewData["Title"] = "Ürün Düzenle";
            var p = await _uow.Products.GetByIdAsync(id);
            if (p == null) return NotFound();
            var vm = new ProductUpdateViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                OldPrice = p.OldPrice,
                Stock = p.Stock,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                Unit = p.Unit,
                IsFeatured = p.IsFeatured,
                Badge = p.Badge,
                IsActive = p.IsActive,
                CaloriesPer100g = p.CaloriesPer100g,
                FatPer100g = p.FatPer100g,
                CarbsPer100g = p.CarbsPer100g,
                ProteinPer100g = p.ProteinPer100g,
                ServingSize = p.ServingSize
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateViewModel vm)
        {
            var p = await _uow.Products.GetByIdAsync(vm.Id);
            if (p == null) return NotFound();
            var changed = new Dictionary<string, string>();
            if (p.Price != vm.Price) changed["Price"] = $"{p.Price}→{vm.Price}";
            if (p.Stock != vm.Stock) changed["Stock"] = $"{p.Stock}→{vm.Stock}";

            p.Name = vm.Name; p.Description = vm.Description; p.Price = vm.Price; p.OldPrice = vm.OldPrice;
            p.Stock = vm.Stock; p.Category = vm.Category; p.ImageUrl = vm.ImageUrl; p.Unit = vm.Unit;
            p.IsFeatured = vm.IsFeatured; p.Badge = vm.Badge; p.IsActive = vm.IsActive;
            p.CaloriesPer100g = vm.CaloriesPer100g; p.FatPer100g = vm.FatPer100g;
            p.CarbsPer100g = vm.CarbsPer100g; p.ProteinPer100g = vm.ProteinPer100g; p.ServingSize = vm.ServingSize;
            p.UpdatedAt = DateTime.Now;

            _uow.Products.Update(p);
            await _uow.SaveChangesAsync();

            await _uow.AuditLogs.AddAsync(new Organi.Models.Entities.AuditLog { Action = "Updated", Description = $"\"{p.Name}\" ürünü güncellendi.", PerformedBy = "Admin" });
            await _uow.SaveChangesAsync();

            await _publisher.PublishAsync(new ProductChangedEvent { ProductId = p.Id, ProductName = p.Name, Action = "Updated", ChangedFields = changed });
            TempData["Success"] = $"\"{p.Name}\" güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _uow.Products.GetByIdAsync(id);
            if (p == null) return NotFound();
            var name = p.Name;
            _uow.Products.Delete(p);
            await _uow.SaveChangesAsync();

            await _uow.AuditLogs.AddAsync(new Organi.Models.Entities.AuditLog { Action = "Deleted", Description = $"\"{name}\" ürünü silindi.", PerformedBy = "Admin" });
            await _uow.SaveChangesAsync();

            await _publisher.PublishAsync(new ProductChangedEvent { ProductId = id, ProductName = name, Action = "Deleted" });
            TempData["Success"] = $"\"{name}\" silindi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Messages()
        {
            ViewData["ActiveMenu"] = "Messages"; ViewData["Title"] = "Mesajlar";
            var messages = await _uow.ContactMessages.GetAllAsync();
            ViewBag.UnreadCount = messages.Count(m => !m.IsRead);
            return View(messages.OrderByDescending(m => m.CreatedAt).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            var msg = await _uow.ContactMessages.GetByIdAsync(id);
            if (msg != null) { msg.IsRead = true; _uow.ContactMessages.Update(msg); await _uow.SaveChangesAsync(); }
            return RedirectToAction(nameof(Messages));
        }

        public async Task<IActionResult> Orders()
        {
            ViewData["ActiveMenu"] = "Orders"; ViewData["Title"] = "Siparişler";
            ViewBag.UnreadCount = (await _uow.ContactMessages.GetAllAsync()).Count(m => !m.IsRead);
            var orders = await _uow.Orders.GetAllAsync();
            return View(orders.OrderByDescending(o => o.CreatedAt).ToList());
        }

        public IActionResult Logs()
        {
            ViewData["ActiveMenu"] = "Logs"; ViewData["Title"] = "Loglar";
            var logPath = Path.Combine(_env.ContentRootPath, "Logs", "audit.log");
            var lines = System.IO.File.Exists(logPath)
                ? System.IO.File.ReadAllLines(logPath).Reverse().Take(100).ToList()
                : new List<string>();
            return View(lines);
        }
    }
}

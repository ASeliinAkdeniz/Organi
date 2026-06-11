using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organi.Data.UnitOfWork;
using Organi.Models.Entities;
using Organi.Models.ViewModels;
using Organi.Patterns.Strategy;

namespace Organi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        public HomeController(IUnitOfWork uow) { _uow = uow; }

        public async Task<IActionResult> Index()
        {
            var all = await _uow.Products.GetAllAsync();
            var active = all.Where(p => p.IsActive).ToList();
            var vm = new HomeViewModel
            {
                FeaturedProducts = active.Where(p => p.IsFeatured || p.Badge != null).Take(8).ToList(),
                NewProducts = active.OrderByDescending(p => p.CreatedAt).Take(8).ToList()
            };
            return View(vm);
        }
    }

    public class ShopController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ShopController(IUnitOfWork uow) { _uow = uow; }

        public async Task<IActionResult> Index(ProductCategory? category, string? search, decimal minPrice = 0, decimal maxPrice = 1000, string? sort = null, int page = 1)
        {
            var all = (await _uow.Products.GetAllAsync()).Where(p => p.IsActive).ToList();

            if (category.HasValue) all = all.Where(p => p.Category == category).ToList();
            if (!string.IsNullOrWhiteSpace(search)) all = all.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            all = all.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

            all = sort switch
            {
                "price_asc" => all.OrderBy(p => p.Price).ToList(),
                "price_desc" => all.OrderByDescending(p => p.Price).ToList(),
                "rating" => all.OrderByDescending(p => p.Rating).ToList(),
                _ => all.OrderByDescending(p => p.CreatedAt).ToList()
            };

            var popular = (await _uow.Products.GetAllAsync()).Where(p => p.IsActive).OrderByDescending(p => p.Rating).Take(5).ToList();
            int pageSize = 9;
            var vm = new ShopViewModel
            {
                Products = all.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = all.Count,
                SelectedCategory = category,
                SearchTerm = search,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sort,
                Page = page,
                PageSize = pageSize,
                PopularProducts = popular
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }

    public class CartController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CartController(IUnitOfWork uow) { _uow = uow; }

        private string SessionId => HttpContext.Session.GetString("CartSessionId") ?? CreateSession();
        private string CreateSession()
        {
            var id = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartSessionId", id);
            return id;
        }

        public async Task<IActionResult> Index()
        {
            var items = (await _uow.CartItems.GetAllAsync())
                .Where(c => c.SessionId == SessionId).ToList();
            foreach (var item in items)
                item.Product = await _uow.Products.GetByIdAsync(item.ProductId);
            var vm = new CartViewModel { Items = items.Where(i => i.Product != null).ToList() };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, double quantity = 1)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null) return NotFound();

            var items = (await _uow.CartItems.GetAllAsync()).Where(c => c.SessionId == SessionId).ToList();
            var existing = items.FirstOrDefault(c => c.ProductId == productId);

            if (existing != null) { existing.Quantity += quantity; _uow.CartItems.Update(existing); }
            else await _uow.CartItems.AddAsync(new CartItem { SessionId = SessionId, ProductId = productId, Quantity = quantity });

            await _uow.SaveChangesAsync();
            TempData["CartSuccess"] = $"{product.Name} sepete eklendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _uow.CartItems.GetByIdAsync(id);
            if (item != null) { _uow.CartItems.Delete(item); await _uow.SaveChangesAsync(); }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(int id, double quantity)
        {
            var item = await _uow.CartItems.GetByIdAsync(id);
            if (item != null && quantity > 0) { item.Quantity = quantity; _uow.CartItems.Update(item); await _uow.SaveChangesAsync(); }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Count()
        {
            var count = (await _uow.CartItems.GetAllAsync()).Count(c => c.SessionId == SessionId);
            return Json(count);
        }

        public async Task<IActionResult> Checkout()
        {
            var items = (await _uow.CartItems.GetAllAsync())
                .Where(c => c.SessionId == SessionId).ToList();
            foreach (var item in items)
                item.Product = await _uow.Products.GetByIdAsync(item.ProductId);
            var vm = new CartViewModel { Items = items.Where(i => i.Product != null).ToList() };
            if (!vm.Items.Any()) return RedirectToAction("Index");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string customerName, string customerEmail, string address)
        {
            var items = (await _uow.CartItems.GetAllAsync())
                .Where(c => c.SessionId == SessionId).ToList();
            foreach (var item in items)
                item.Product = await _uow.Products.GetByIdAsync(item.ProductId);
            items = items.Where(i => i.Product != null).ToList();
            if (!items.Any()) return RedirectToAction("Index");

            decimal subtotal = items.Sum(i => i.Product!.Price * (decimal)i.Quantity);
            decimal shipping = subtotal >= 99m ? 0m : 9.99m;

            var order = new Organi.Models.Entities.Order
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                Address = address,
                TotalPrice = subtotal + shipping,
                ShippingFee = shipping,
                Status = Organi.Models.Entities.OrderStatus.Pending,
                Items = items.Select(i => new Organi.Models.Entities.OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product!.Price
                }).ToList()
            };

            await _uow.Orders.AddAsync(order);

            // Sepeti temizle
            foreach (var item in items)
                _uow.CartItems.Delete(item);

            await _uow.SaveChangesAsync();

            TempData["OrderSuccess"] = order.Id.ToString();
            return RedirectToAction("OrderConfirm");
        }

        public IActionResult OrderConfirm()
        {
            ViewBag.OrderId = TempData["OrderSuccess"];
            return View();
        }
    }

    public class CompareController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CompareController(IUnitOfWork uow) { _uow = uow; }

        public async Task<IActionResult> Index()
        {
            var ids = HttpContext.Session.GetString("CompareIds")?.Split(',').Select(int.Parse).ToList() ?? new();
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var p = await _uow.Products.GetByIdAsync(id);
                if (p != null) products.Add(p);
            }
            return View(new CompareViewModel { Products = products });
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var ids = HttpContext.Session.GetString("CompareIds")?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new();
            if (!ids.Contains(productId) && ids.Count < 4) ids.Add(productId);
            HttpContext.Session.SetString("CompareIds", string.Join(',', ids));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var ids = HttpContext.Session.GetString("CompareIds")?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new();
            ids.Remove(productId);
            HttpContext.Session.SetString("CompareIds", string.Join(',', ids));
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Organi.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context) { _context = context; _dbSet = context.Set<T>(); }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}

namespace Organi.Data.UnitOfWork
{
    using Organi.Data.Repositories;
    using Organi.Models.Entities;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<ContactMessage> ContactMessages { get; }
        IRepository<AuditLog> AuditLogs { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Product>? _products;
        private IRepository<CartItem>? _cartItems;
        private IRepository<Order>? _orders;
        private IRepository<OrderItem>? _orderItems;
        private IRepository<ContactMessage>? _contactMessages;
        private IRepository<AuditLog>? _auditLogs;

        public UnitOfWork(AppDbContext context) { _context = context; }

        public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
        public IRepository<CartItem> CartItems => _cartItems ??= new Repository<CartItem>(_context);
        public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);
        public IRepository<ContactMessage> ContactMessages => _contactMessages ??= new Repository<ContactMessage>(_context);
        public IRepository<AuditLog> AuditLogs => _auditLogs ??= new Repository<AuditLog>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}

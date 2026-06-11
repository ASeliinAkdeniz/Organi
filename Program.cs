using Microsoft.EntityFrameworkCore;
using Organi.Data;
using Organi.Data.UnitOfWork;
using Organi.Patterns.Observer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSession(o => { o.IdleTimeout = TimeSpan.FromHours(2); o.Cookie.HttpOnly = true; });

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IProductEventPublisher>(sp =>
{
    var pub = new ProductEventPublisher();
    pub.Subscribe(new AdminNotificationObserver());
    pub.Subscribe(new AuditLogObserver(sp.GetRequiredService<IWebHostEnvironment>()));
    return pub;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("admin", "Admin/{action=Dashboard}/{id?}", new { controller = "Admin" });
app.Run();

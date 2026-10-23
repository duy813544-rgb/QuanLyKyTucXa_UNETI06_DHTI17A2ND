using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(4);
});
var connectionString = builder.Configuration.GetConnectionString("KyTucXa")
    ?? "Server=localhost;Database=QL_KY_TUC_XA;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

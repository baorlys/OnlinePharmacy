using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<OnlinePharmacyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "product",
        pattern: "{controller=Product}/{action=Index}/san-pham/{meta}/{name?}");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "blog",
        pattern: "{controller=Blog}/{action=Index}/bai-viet");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();

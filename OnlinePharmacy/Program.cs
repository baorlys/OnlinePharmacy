using Autofac.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;
using System.Security.Policy;
using CKSource.CKFinder.Connector.Config;
using CKSource.CKFinder.Connector.Core.Builders;
using CKSource.CKFinder.Connector.Core.Logs;
using CKSource.CKFinder.Connector.Host.Owin;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddRazorPages();
builder.Services.AddDbContext<OnlinePharmacyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60 * 10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");



app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
});


app.Run();

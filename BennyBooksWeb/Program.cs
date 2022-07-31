using BennyBooksWeb.DataAccess;
using BennyBooks.DataAccess.Repository;
using BennyBooks.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Can be used in entire program
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection") // Gets connection string from appsettings.json, DefualtCon... is can be named anything based on json
    ));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// To update the database run Nuget Pakckage Manger in Console mode then use -- add-migration {Class name you want}
// Make sure default project drop down in the bottom is set to DataAccess and used remove-migraton to get rid of old one after deleting it
// To apply update, including creating a table if it doesn't exist use -- update-database

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Dependancies can now be made in UnityOfWork
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // makes it so changes made by razor (add navbar) can be updated during runtime

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

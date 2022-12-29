using BennyBooks.DataAccess.Repository;
using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooksWeb.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BennyBooks.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using BennyBooks.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Can be used in entire program
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection") // Gets connection string from appsettings.json, DefualtCon... is can be named anything based on json
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
// To update the database run Nuget Pakckage Manger in Console mode then use -- add-migration {Class name you want}
// Make sure default project drop down in the bottom is set to DataAccess and used remove-migraton to get rid of old one after deleting it
// To apply update, including creating a table if it doesn't exist use -- update-database

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Dependancies can now be made in UnityOfWork
builder.Services.AddScoped<IDbInitializer, DbInitializer> ();
builder.Services.AddSingleton<IEmailSender, EmailSender>(); // Resolves issue mentioned in the EmailSender method
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // makes it so changes made by razor (add navbar) can be updated during runtime
builder.Services.ConfigureApplicationCookie(options => // Resolves redirection issue when a user tries to load page where they are no longer  logged in that required Authorization
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout"; // default does not contain Identity
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

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
SeedDatabase();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages(); // Makes it so program can find pages properly for the asp-page calls

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Makes it so our database is created and seeded with data when ran
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        //var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        //var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}

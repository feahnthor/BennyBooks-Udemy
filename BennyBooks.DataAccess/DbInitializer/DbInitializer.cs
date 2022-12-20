using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using BennyBooks.Utility;
using BennyBooksWeb.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BennyBooks.DataAccess.DbInitializer
{
    // This is the interface for the DbInitializer class
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager; // Helper method
            _db = db;
        }

        // implement Initialize() method
        public void Initialize()
        {
            // apply migrations if they are not applied
            try
            {
                // if there are no migrations, apply them
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate(); // makes it so we no longer have to run the command "update-database" in the package manager console
                }
            }
            catch (Exception ex)
            {
            }

            // create roles if they are not created yet
            //use instead of GetAwaiter().GetResult()  https://stackoverflow.com/questions/34549641/async-await-vs-getawaiter-getresult-and-
            bool adminRoleExistInDb = _roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult();
            if (!adminRoleExistInDb) // If the role does not exists, add it to the database default values from SD.cs
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Super_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Individual)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

                ApplicationUser user = new ApplicationUser
                {
                    UserName = "mnmathgeek1@gmail.com", // can't log in unless email is the same. need to fix
                    Email = "mnmathgeek1@gmail.com",
                    Name = "The Martial God",
                    PhoneNumber = "1234567890",
                    StreetAddress = "test 123 Ave",
                    State = "MN",
                    PostalCode = "12345",
                    City = "Minneapolis",
                    CreateDate = DateTime.Now,
                };

                // if roles are not created, then we will create admin user as well
                var result = _userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult(); //passwords needs to meet certain types by IdentityUser

                if (result.Succeeded)
                {
                    ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "mnmathgeek1@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Super_Admin).GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
                else
                {
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Description);
                    }

                    user = new ApplicationUser
                    {
                        UserName = "martialgod",
                        Email = "mnmathgeek1@gmail.com",
                        Name = "The Martial God",
                        PhoneNumber = "1234567890",
                        StreetAddress = "test 123 Ave",
                        State = "MN",
                        PostalCode = "12345",
                        City = "Minneapolis",
                        CreateDate = DateTime.Now,
                    };
                    _userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult();
                    ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "mnmathgeek1@gmail.com");
                }


                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
            
        }
    }
}

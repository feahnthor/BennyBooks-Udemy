using BennyBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BennyBooksWeb.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext // Uses EntityFrameWorkCore to try associating our Db
    {
        // Use IdentityDbContext to add new scafolding to our Web project Identity.EntityFrameworkCore
        // Clear nuget packages with errors Scaffolding for Identity
        // https://social.msdn.microsoft.com/Forums/en-US/07c93e8b-5092-4211-80e6-3932d87664c3/always-got-this-error-when-scaffolding-suddenly-8220there-was-an-error-running-the-selected-code?forum=aspdotnetcore

        /* CREATING A NEW DATABASE THROUGH MIGRATION
         * 1. Delete the Migrations folder and the database from SQL
         * 2. in Package Manger Console run 
         *         > add-migration {Migration Name}
         * 3.      > update-database
         */
        // Get DbContextOptions containing ApplicationDbContext and name it options, then call the base class and pass it options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // we use BennyBooksWeb.Models so that the program is aware that a class called Category exists
        // Creates category table using the properties for the class, ensuring Id is a Key
        public DbSet<Category> Categories { get; set; } 
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    }
}

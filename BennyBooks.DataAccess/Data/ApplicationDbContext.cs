using BennyBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Makes it so our DbContext is available when adding Identity Scaffolding
using Microsoft.EntityFrameworkCore;

namespace BennyBooksWeb.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext // Uses EntityFrameWorkCore to try associating our Db
    {
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
    }
}

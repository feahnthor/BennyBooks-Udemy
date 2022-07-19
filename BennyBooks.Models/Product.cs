using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Display(Name = "Page Count")]
        public int PageCount { get; set; }
        [Required]
        public string Author { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        
        /*
         * Reviews can be very big if there are 10,000 books and 1,000, that will be close to
         * half a million. So creating a new database after limiting ammount or one for each book would be better
         * but would be more */
        //public int ReviewCount { get; set; }

        // Add Tags

        [Required]
        [Range(0, 100000)]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }

        [Required]
        [Range(0, 100000)]
        [Display(Name = "Price for 50-100")]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(0, 100000)]
        public double Price100 { get; set; }

        // Since this is the same as Category with Id added, it will be a foreign key that is referencing the Category Model
        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")] // Manual way to set foreign key in case the name is not similar to the class + field
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public Guid CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}

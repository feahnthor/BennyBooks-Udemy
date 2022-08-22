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
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; } // IdentityUser actually has an string for an Id not Guid
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Range(1, 100000, ErrorMessage = "Please enter a value between 1 and 100000")]
        public int Count { get; set; }
        [NotMapped] // Should not be added to our database
        public double Price { get; set; }
    }
}

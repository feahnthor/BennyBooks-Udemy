using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1, 100000, ErrorMessage = "Please enter a value between 1 and 100000")]
        public int Count { get; set; }
    }
}

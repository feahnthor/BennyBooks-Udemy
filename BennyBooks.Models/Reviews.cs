using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models
{
    public class Reviews
    {
        [Required]
        public Guid Id { get; set; }
        public int TotalReviews { get; set; }
        public string ReviewsText { get; set; } // Might need to make it serializable
        
        // Might need to add User relation
        public int OneStar { get; set; }
        public int TwoStar { get; set; }
        public int ThreeStar { get; set; }
        public int FourStar { get; set; }
        public int FiveStar { get; set; }
        public int Likes { get; set; }

        public double RangeFiveStar { get; set; }
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}

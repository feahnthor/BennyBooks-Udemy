using System.ComponentModel;
using System.ComponentModel.DataAnnotations; // using Entityframework for the database 

namespace BennyBooks.Models
{
    public class Category
    {
        [Key] // Makes Id the main key
        public Guid Id { get; set; }

        [Required] // Makes it so that Name is not a nullable property
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(-10000, 10000, ErrorMessage = "Display Order must be between -10,000 and 10,000")]
        public int DisplayOrder { get; set; }
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}

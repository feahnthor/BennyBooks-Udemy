using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models
{
    [Serializable]
    public class Portfolio
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string TechnologiesUsed { get; set; }
        public string ProjectUrl { get; set; }
    }
}

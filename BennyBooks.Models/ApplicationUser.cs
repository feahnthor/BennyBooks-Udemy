using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastLoginAttemptedDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public int FailedPasswordAttempt { get; set; }
        public bool IsPasswordExpired { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsLockedOut { get; set; }
    }
}

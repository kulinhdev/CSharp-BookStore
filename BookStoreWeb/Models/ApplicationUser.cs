using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

    }
}
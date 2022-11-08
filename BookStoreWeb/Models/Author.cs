using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreWeb.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(6)]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [DisplayName("Avatar")]
        [ValidateNever]
        public string AvatarUrl { get; set; }

        [Required]
        [MinLength(4)]
        [DisplayName("Author URL")]
        public string Slug { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }


    }
}

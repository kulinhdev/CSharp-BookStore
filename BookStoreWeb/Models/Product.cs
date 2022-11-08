using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        public string Name { get; set; }

        [Required, Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "float(7, 2)")]
        public float Price { get; set; }

        [Required, Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "float(7, 2)")]
        public float SalePrice { get; set; }

        [Required]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string ImagePath { get; set; }

        [Required]
        [MinLength(4)]
        [DisplayName("Product URL")]
        public string Slug { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [ValidateNever]
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
    }
}

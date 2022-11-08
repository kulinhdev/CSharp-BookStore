using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        [DisplayName("Category URL")]
        public string Slug { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}

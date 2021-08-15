using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JMBookStore.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Cover Name")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

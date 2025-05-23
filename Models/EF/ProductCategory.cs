﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory : CommonAbstract
    {
        public ProductCategory()
        {
            this.Products = new HashSet<Product>();


        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }

        public string? Description { get; set; }
        [StringLength(150)]
        public string? Icon { get; set; }
        [StringLength(250)]


        public string? SeoTitle { get; set; }
        [StringLength(500)]

        public string? SeoDescription { get; set; }
        [StringLength(250)]

        public string? SeoKeywords { get; set; }
        [InverseProperty("ProductCategory")]

        public ICollection<Product>? Products { get; set; }

    }
}

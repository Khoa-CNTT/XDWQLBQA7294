using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models.EF;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(250)]
        public string? Title { get; set; }
        public string? Alias { get; set; }

        public string ?ProductCode { get; set; }
        public string ?Description { get; set; }
        public string ?Detail { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int Quantiry { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }

        public int ProductCategoryId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeyWords { get; set; }

        public virtual ProductCategory? Category { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models.EF;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        public Product() { 
            this.ProductImages = new HashSet<ProductImage>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        [StringLength(50)]
        public string ?ProductCode { get; set; }
        public string ?Description { get; set; }
        public string ?Detail { get; set; }
        public string? Image { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}đ", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}đ", ApplyFormatInEditMode = true)]
        public decimal? PriceSale { get; set; }
        public int? Quantiry { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }

        public int? ProductCategoryId { get; set; }
        [StringLength (250)]
        public string? SeoTitle { get; set; }
        [StringLength(50)]
        public string? SeoDescription { get; set; }
        [StringLength(250)]
        public string? SeoKeyWords { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory? ProductCategory { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }


    }
}

using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models;
using EShopp.Models.EF;

namespace Eshopp.Models.EF
{
    [Table("tb_Category")]
    public class Category : CommonAbstract
    {
        public Category()
        {
            this.News = new List<News>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không để trống")]
        [StringLength(150)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }  // Cho phép NULL

        [StringLength(150)]
        public string? SeoTitle { get; set; }  // Cho phép NULL

        [StringLength(250)]
        public string? SeoDescription { get; set; }  // Cho phép NULL

        [StringLength(150)]
        public string? SeoKeywords { get; set; }  // Cho phép NULL
        public bool IsActive { get; set; }

        public int Position { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Posts>? Posts { get; set; }
    }
}

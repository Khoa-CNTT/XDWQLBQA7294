using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Eshopp.Models.EF;
using EShopp.Models;
using System.Web.Mvc;

namespace Eshopp.Models.EF
{
    [Table("tb_Posts")]
    public class Posts : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required ]
        [StringLength(150)]
        public string ?Title { get; set; }
        [StringLength(150)]

        public string ?Alias { get; set; }

        public string? Description { get; set; }
        [AllowHtml]
        public string? Detail { get; set; }
        [StringLength(250)]

        public string? Image { get; set; }
        public int CategoryId { get; set; }
        [StringLength(250)]

        public string ?SeoTitle { get; set; }
        [StringLength(250)]

        public string? SeoDescription { get; set; }
        public string? SeoKeyWords { get; set; }
        public bool IsActive { get; set; }

        public virtual Category ?Category { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Eshopp.Models.EF;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Posts")]
    public class Posts : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string ?Title { get; set; }
        public string ?Alias { get; set; }

        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public string ?SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeyWords { get; set; }
        public bool IsActive { get; set; }

        public virtual Category ?Category { get; set; }
    }
}

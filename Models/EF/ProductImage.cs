﻿    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using EShopp.Models;

    namespace EShopp.Models.EF
    {
        [Table("tb_ProductImage")]
        public class ProductImage
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }
            public int ProductId { get; set; }
            public string? Image { get; set; }
            public bool IsDefault { get; set; }
            public virtual Product? Product { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }

    }
}

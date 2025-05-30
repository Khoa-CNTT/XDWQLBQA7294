﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Adv")]
    public class Adv : CommonAbstract
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(500)]

        public string Description { get; set; }
        [StringLength(500)]

        public string Image { get; set; }
        [StringLength(500)]

        public string Link { get; set; }
        public int Type { get; set; }
    }
}

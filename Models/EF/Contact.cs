﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        public string? Name { get; set; }
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]

        public string?Email { get; set; }
        public string? WebSite { get; set; }
        [StringLength(4000)]
        public string? Message { get; set; }
        public string? IsRead { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShopp.Models;

namespace EShopp.Models.EF
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]

        public string CustomerName { get; set; }
        [Required]

        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment {  get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

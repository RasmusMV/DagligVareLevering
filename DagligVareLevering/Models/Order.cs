using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DagligVareLevering.Models
{
    public class Order
    {
        
        public Order()
        {
            TimeOfOrder = DateTime.Now;
            OrderLines = new List<OrderLine>();
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;

            foreach (var line in OrderLines)
            {
                total += line.GetLineTotal();
            }

            return total;
        }
      

        public Order(ICollection<OrderLine> orderLines, User user, DateTime expectedDeliveryTime, string adress)
        {
            OrderLines = orderLines;
            User = user;
            TimeOfOrder = DateTime.Now;
            ExpectedDeliveryTime = expectedDeliveryTime;
            Adress = adress;
            OrderLines = new List<OrderLine>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public DateTime TimeOfOrder { get; set; }

        public DateTime ExpectedDeliveryTime { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        [Required, MaxLength(100)]
        public string Adress { get; set; }
    }
}

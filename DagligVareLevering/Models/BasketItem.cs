using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DagligVareLevering.Models
{
    public class BasketItem
    {
        public BasketItem() { } 

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BasketItemId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

    }
}

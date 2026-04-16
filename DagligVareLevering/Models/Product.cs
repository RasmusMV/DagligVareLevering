using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Models
{
    public class Product
    {
        public Product() { }

        public Product(string name, decimal price, string information, Store store)
        {
            Name = name;
            Price = price;
            Information = information;
            Store = store;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product must have a name"), MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product must have a price")]
        [Range(typeof(decimal),"0", "10000", ErrorMessage = "The price must be between {1} and {2}")]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product must have a description"), MaxLength(500)]
        public string Information { get; set; }

        [Required]
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }

    }
}

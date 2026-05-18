using System.ComponentModel.DataAnnotations;

namespace DagligVareLevering.Models.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Product must have a name"), MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product must have a price")]
        [Range(typeof(decimal), "0", "10000", ErrorMessage = "The price must be between {1} and {2}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product must have a description"), MaxLength(500)]
        public string Information { get; set; }

        [Required(ErrorMessage = "Store blev ikke fundet, valgte du en gyldig store?")]
        public int StoreId { get; set; }



    }
}

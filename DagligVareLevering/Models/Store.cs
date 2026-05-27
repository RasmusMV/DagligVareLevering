using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DagligVareLevering.Models
{
    public class Store
    {

        public Store() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Store must have a name"), MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Store must have a adress"), MaxLength(100)]
        public string Adress { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DagligVareLevering.Models
{
    public class User
    {

        public User() { }

        public User(string name, string adress, string phonenumber, string email)
        {
            Name = name;
            Adress = adress;
            Phonenumber = phonenumber;  
            Email = email;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User must have a name"), MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "User must have a password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User must have a adress"), MaxLength(100)]
        public string Adress { get; set; }

        [Required(ErrorMessage = "User must give a phonenumber")]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Phonenumber must be 8 digits")]
        public string Phonenumber { get; set; }

        [Required(ErrorMessage = "User must have a email"), MaxLength(100)]
        public string Email { get; set; }

        public virtual ICollection<BasketItem>? Basket { get; set; }

        public virtual ICollection<Order>? OrderHistory { get; set; }



    }
}

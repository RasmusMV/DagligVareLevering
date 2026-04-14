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

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Adress { get; set; }

        public string Phonenumber { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Product> Basket { get; set; }

        public virtual ICollection<Order> OrderHistory { get; set; }



    }
}

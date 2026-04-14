namespace DagligVareLevering.Models
{
    public class Order
    {
        
        public Order()
        {
            TimeOfOrder = DateTime.Now;
        }

        public Order(List<Product> products, User user, DateTime expectedDeliveryTime, string adress)
        {
            Products = products;
            User = user;
            TimeOfOrder = DateTime.Now;
            ExpectedDeliveryTime = expectedDeliveryTime;
            Adress = adress;
        }

        public int OrderId { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public User User { get; set; }

        public DateTime TimeOfOrder { get; set; }

        public DateTime ExpectedDeliveryTime { get; set; }

        public string Adress { get; set; }
    }
}

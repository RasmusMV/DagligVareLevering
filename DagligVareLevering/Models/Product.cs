namespace DagligVareLevering.Models
{
    public class Product
    {
        public Product() { }

        public Product(string name, float price, string information, Store store)
        {
            Name = name;
            Price = price;
            Information = information;
            Store = store;
        }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string Information { get; set; }

        public Store Store { get; set; }
    }
}

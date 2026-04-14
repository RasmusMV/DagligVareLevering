namespace DagligVareLevering.Models
{
    public class Store
    {

        public Store() { }

        public Store(string name, string adress)
        {
            Name = name;
            Adress = adress;
        }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public string Adress { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}

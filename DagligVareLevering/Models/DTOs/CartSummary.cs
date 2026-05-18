namespace DagligVareLevering.Models.DTOs
{
    public class CartSummary
    {
        public List<BasketItem> Items { get; set; }
        public decimal ItemsTotalPrice {  get; set; }
        public decimal DeliveryPrice { get; set; }
        public decimal TotalWithDelivery { get; set; }
    }
}

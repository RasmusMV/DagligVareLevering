using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public class CartEventService
    {
        // Eventet bliver udløst, når en vare lægges i kurven
        public event Action<BasketItem>? CartItemAdded;

        // Kaldes når systemet skal give besked om, at en vare er lagt i kurven
        public void OnCartItemAdded(BasketItem basketItem)
        {
            CartItemAdded?.Invoke(basketItem);
        }
    }
}

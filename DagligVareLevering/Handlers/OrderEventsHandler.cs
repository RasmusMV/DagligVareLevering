using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Handlers
{
    public class OrderEventsHandler
    {
        public OrderEventsHandler(IOrderService orderService)
        {
            orderService.OrderTaken += OnOrderTaken;
        }

        private async void OnOrderTaken(object? sender, Order order)
        {
            Console.WriteLine($"Du har taget ordre #{order.OrderId} som skal leveres til {order.Adress}");
        }
    }
}

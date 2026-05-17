using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Handlers
{
    public class OrderEventsHandler
    {
        public OrderEventsHandler(IOrderService orderService)
        {
            orderService.OrderTaken += OnOrderTaken;
        }

        private void OnOrderTaken(object? sender, Order order)
        {

        }
    }
}

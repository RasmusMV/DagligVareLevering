using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Handlers
{
    public class OrderEventsHandler
    {
        private readonly IRepository<Notification> _notificationRepository;
        public OrderEventsHandler(IRepository<Notification> notificationRepository, IOrderService orderService)
        {
            _notificationRepository = notificationRepository;
            orderService.OrderTaken += OnOrderTaken;
        }

        private void OnOrderTaken(object? sender, Order order)
        {
            _notificationRepository.AddObjectAsync(new Notification
            {
                UserId = order.WorkerId.Value,
                Message = $"Du har taget ordre #{order.OrderId} som skal leveres til {order.Adress}"
            }).GetAwaiter().GetResult();
        }
    }
}

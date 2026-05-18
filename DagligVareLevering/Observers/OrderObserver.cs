using DagligVareLevering.Models;
using DagligVareLevering.Observers.Interfaces;
using DagligVareLevering.Repositories.Interfaces;

namespace DagligVareLevering.Observers
{
    public class OrderObserver : IOrderObserver
    {
        private readonly IRepository<Notification> _notificationRepository;

        public OrderObserver(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task OnOrderDeliveredAsync(Order order)
        {
            await _notificationRepository.AddObjectAsync(new Notification
            {
                UserId = order.UserId,
                Message = $"Din ordre #{order.OrderId} er blevet leveret"
            });
        }


    }
}

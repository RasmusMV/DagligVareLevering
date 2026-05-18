using DagligVareLevering.Models;

namespace DagligVareLevering.Observers.Interfaces
{
    public interface IOrderObserver
    {
        Task OnOrderDeliveredAsync(Order order);
    }
}

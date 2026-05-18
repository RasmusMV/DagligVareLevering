using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Observers.Interfaces;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service;
using Moq;

namespace DagligVareLeveringTest;

[TestClass]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IRepository<OrderLine>> _orderLineRepositoryMock;
    private Mock<IEnumerable<IOrderObserver>> _observerMock;
    private OrderService _orderService;

    [TestInitialize]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderLineRepositoryMock = new Mock<IRepository<OrderLine>>();
        
        var observer = new List<IOrderObserver>();

        _orderService = new OrderService(
           _orderRepositoryMock.Object, 
           _orderLineRepositoryMock.Object, 
           observer
            );
    }


    [TestMethod]
    public async Task MarkOrderAsDeliveredAsync_ValidOrder_SetStatusToDelivered()
    {
        //Arrange
        var order = new Order() { OrderId = 1, WorkerId = 1, Status = OrderStatus.OutForDelivery };
        _orderRepositoryMock
            .Setup(o => o.GetObjectByIdAsync(1))
            .ReturnsAsync(order);

        //Act
        await _orderService.MarkOrderAsDeliveredAsync(1, 1);

        //Assert
        Assert.AreEqual(OrderStatus.Delivered, order.Status);
    }
}

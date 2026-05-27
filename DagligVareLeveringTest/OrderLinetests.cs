using DagligVareLevering.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DagligVareLeveringTest
{
    [TestClass]
    public class OrderLineTests
    {
        [TestMethod]
        public void GetLineTotal_ReturnsCorrectPrice()
        {
            Product product = new Product
            {
                Name = "Mælk",
                Price = 12m
            };

            OrderLine orderLine = new OrderLine
            {
                Product = product,
                Quantity = 2
            };

            decimal result = orderLine.GetLineTotal();

            Assert.AreEqual(24m, result);
        }
    }
}
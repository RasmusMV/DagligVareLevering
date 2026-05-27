using DagligVareLevering.Handlers;
using DagligVareLevering.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DagligVareLeveringTest
{
    [TestClass]
    public class CartEventHandlerTest
    {
        [TestMethod]
        public void OnCartItemAdded_ShouldTriggerCartItemAddedEvent()
        {
            // Arrange
            // Opretter event-servicen og en testvare til kurven
            CartEventHandler eventHandler = new CartEventHandler();

            BasketItem basketItem = new BasketItem
            {
                BasketItemId = 1,
                UserId = 1,
                ProductId = 1,
                Quantity = 1
            };

            bool eventWasTriggered = false;

            // Lytter på eventet og ændrer bool-værdien, hvis eventet bliver udløst
            eventHandler.CartItemAdded += item =>
            {
                eventWasTriggered = true;
            };

            // Act
            // Kalder metoden, som skal udløse eventet
            eventHandler.OnCartItemAdded(basketItem);

            // Assert
            // Testen består, hvis eventet blev udløst
            Assert.IsTrue(eventWasTriggered);
        }
    }
}

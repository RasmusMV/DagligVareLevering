namespace DagligVareLeveringTest;
using DagligVareLevering.Models;
using DagligVareLevering.Pages.Product;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

[TestClass]
public class ProductTest
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public async Task OnPostAddToCartAsync_UserNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            var model = new GroceriesModel(null, null, null);

            // Fake HttpContext
            var httpContext = new DefaultHttpContext();

            model.PageContext = new PageContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await model.OnPostAddToCartAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirectResult = result as RedirectToPageResult;

            Assert.AreEqual("/Login", redirectResult.PageName);
        }
    }
}
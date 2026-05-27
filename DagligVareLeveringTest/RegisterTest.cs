using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DagligVareLeveringTest
{
    namespace DagligVareLeveringTest
    {
        [TestClass]
        public class RegisterTest
        {
            [TestMethod]
            public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
            {
                //opretter en fake version af IUserService
                var mockUserService = new Mock<IUserService>();

                //opretter page modellen og giver den fake service med
                var model = new RegisterModel(mockUserService.Object);

                //tilføjer en fejl til ModelState
                //dette gør at ModelState.IsValid bliver false
                model.ModelState.AddModelError("Error", "Test error");

                // kalder OnPostAsync metoden
                var result = await model.OnPostAsync();

                //tjekker at resultatet er et PageResult
                //det betyder at brugeren bliver på samme side
                Assert.IsInstanceOfType(result, typeof(PageResult));
            }
            [TestMethod]
            public async Task OnPostAsync_ModelStateValid_RedirectsToLogin()
            {
                //opretter en falsk version af IUserService
                var mockUserService = new Mock<IUserService>();

                //opretter page modellen og giver den falske service med
                var model = new RegisterModel(mockUserService.Object);

                //opretter en testbruger
                model.User = new User
                {
                    Email = "test@test.com",
                    Password = "Password"
                };

                //kalder OnPostAsync metoden
                var result = await model.OnPostAsync();

                //tjekker at resultatet er et redirect
                Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

                //konverterer resultatet så vi kan undersøge redirectet
                var redirectResult = result as RedirectToPageResult;

                //tjekker at brugeren bliver sendt til login-siden
                Assert.AreEqual("/UserRelated/Login", redirectResult.PageName);

                //tjekker at RegisterUserAsync blev kaldt præcis én gang
                mockUserService.Verify(
                    x => x.RegisterUserAsync(model.User),
                    Times.Once);
            }
        }
    }
}

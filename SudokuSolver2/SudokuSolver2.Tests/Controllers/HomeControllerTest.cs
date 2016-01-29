using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver2;
using SudokuSolver2.Controllers;

namespace SudokuSolver2.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Solve()
        {
            var controller = new HomeController();

            PartialViewResult result = controller.Solve("");

            Assert.IsNotNull(result);
        }
    }
}

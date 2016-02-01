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
        [TestMethod, TestCategory("Initalizers")]
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
            string VALID_STARTING_PUZZLE = "6x7x81xxxx5x9xxx32xxxx5xxxx2x6xxxxx3x74xxx95x8xxxxx7x4xxxx1xxxx94xxx2x7xxxx74x2x8";
            PartialViewResult result = controller.Solve(VALID_STARTING_PUZZLE);
            Assert.IsNotNull(result);
        }
    }
}

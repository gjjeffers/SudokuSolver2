using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver2.Models.Tests
{
    [TestClass]
    public class NodeTests
    {
        #region InitializerTests
        [TestMethod, TestCategory("Initalizers")]
        public void NodeTest()           
        {
            Node emptyTest = new Node();
            Assert.AreEqual(0, emptyTest.Row);
            Assert.AreEqual(0, emptyTest.Column);
            Assert.AreEqual(0, emptyTest.Block);
            Assert.AreEqual(0, emptyTest.Value);
            Assert.AreEqual(9, emptyTest.Possibilities.Count);

            Node coordTest = new Node() { Row = 1, Column = 2, Block = 3 };
            Assert.AreEqual(1, coordTest.Row);
            Assert.AreEqual(2, coordTest.Column);
            Assert.AreEqual(3, coordTest.Block);
            Assert.AreEqual(0, coordTest.Value);
            Assert.AreEqual(9, coordTest.Possibilities.Count);

            Node valueTest = new Node() { Row = 1, Column = 2, Block = 3, Value = 4 };
            Assert.AreEqual(1, valueTest.Row);
            Assert.AreEqual(2, valueTest.Column);
            Assert.AreEqual(3, valueTest.Block);
            Assert.AreEqual(4, valueTest.Value);
            Assert.AreEqual(9, coordTest.Possibilities.Count);

        }

        #endregion

        #region MethodTests
        [TestMethod,TestCategory("Methods")]
        public void SetValueTest()
        {
            Node valueTest = new Node();
            List<string> events = new List<string>();
            valueTest.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                events.Add(e.PropertyName);
            };
            valueTest.SetValue(1);
            Assert.AreEqual(0, valueTest.Possibilities.Count, "Possibilities not cleared");
            Assert.AreEqual(1, valueTest.Value, "Value not set");
            Assert.AreEqual("Value", events[0], "No event was raised");


        }
        #endregion

    }
}
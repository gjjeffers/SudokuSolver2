using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver2.Models.Tests
{
    [TestClass]
    public class NodeGroupTests
    {
        #region MethodTests
        [TestMethod]
        public void PossibilityCountTest()
        {
            List<Node> possTestNodes = new List<Node>();
            possTestNodes.Add(new Node());
            possTestNodes.Add(new Node());
            possTestNodes.Add(new Node());

            NodeGroup possTest = new NodeGroup(possTestNodes);
            possTest.Nodes[0].SetValue(1);
            Assert.AreEqual(0, possTest.PossibilityCount(1));
            Assert.AreEqual(2, possTest.PossibilityCount(2));

        }

        [TestMethod]
        public void ValueSetEventTest()
        {
            List<Node> valueTestNodes = new List<Node>();
            valueTestNodes.Add(new Node());
            valueTestNodes.Add(new Node());

            NodeGroup valueTest = new NodeGroup(valueTestNodes);
            valueTest.Nodes[0].SetValue(1);
            Assert.IsFalse(valueTest.Nodes[0].Possibilities.Contains(1));
            Assert.IsFalse(valueTest.Nodes[1].Possibilities.Contains(1));
        }

        [TestMethod]
        public void SolveTest()
        {
            List<Node> solveTestNodes = new List<Node>();
            for(int i = 0; i < 9; i++)
            {
                solveTestNodes.Add(new Node());

            }
            solveTestNodes[0].Possibilities = new List<int>() { 1 };
            solveTestNodes[1].Possibilities = new List<int>() { 1, 2 };
            solveTestNodes[2].Possibilities = new List<int>() { 1, 2, 3 };
            solveTestNodes[3].Possibilities = new List<int>() { 1, 3, 4 };
            solveTestNodes[4].Possibilities = new List<int>() { 1, 4, 5 };
            solveTestNodes[5].Possibilities = new List<int>() { 2, 5, 6 };
            solveTestNodes[6].Possibilities = new List<int>() { 2, 6, 7 };
            solveTestNodes[7].Possibilities = new List<int>() { 2, 7, 8 };
            solveTestNodes[8].Possibilities = new List<int>() { 2, 8, 9 };

            NodeGroup solveTest = new NodeGroup(solveTestNodes);
            solveTest.Solve();
            Assert.AreEqual(1, solveTest.Nodes[0].Value);
            Assert.AreEqual(2, solveTest.Nodes[1].Value);
            Assert.AreEqual(3, solveTest.Nodes[2].Value);
            Assert.AreEqual(4, solveTest.Nodes[3].Value);
            Assert.AreEqual(5, solveTest.Nodes[4].Value);
            Assert.AreEqual(6, solveTest.Nodes[5].Value);
            Assert.AreEqual(7, solveTest.Nodes[6].Value);
            Assert.AreEqual(8, solveTest.Nodes[7].Value);
            Assert.AreEqual(9, solveTest.Nodes[8].Value);
        }
        #endregion
    }
}
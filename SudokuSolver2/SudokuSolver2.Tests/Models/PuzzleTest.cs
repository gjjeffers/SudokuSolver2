using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver2.Models;
using System.Text;

namespace SudokuSolver2.Tests.Models
{
    [TestClass]
    public class PuzzleTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Starting Puzzle String" )]
        public void EmptyPuzzle()
        {
            var emptyTest = new Puzzle("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Starting Puzle String")]
        public void ShortPuzzle()
        {
            var shortTest = new Puzzle("123456789");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Starting Puzle String")]
        public void LongPuzzle()
        {
            var longPuzzle = new StringBuilder();
            while (longPuzzle.Length < 82)
            {
                longPuzzle.Append("1");
            }
            var longTest = new Puzzle(longPuzzle.ToString());
        }

        [TestMethod]
        public void CorrectPuzzle()
        {
            var longPuzzle = new StringBuilder();
            while (longPuzzle.Length < 81)
            {
                longPuzzle.Append("1");
            }
            var correctTest = new Puzzle(longPuzzle.ToString());
            Assert.IsNotNull(correctTest);
        }

    }
}

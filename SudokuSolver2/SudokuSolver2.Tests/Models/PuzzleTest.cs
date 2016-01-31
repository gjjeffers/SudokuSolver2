using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver2.Models;
using System.Text;

namespace SudokuSolver2.Tests.Models
{
    [TestClass]
    public class PuzzleTest
    {
        const string VALID_STARTING_PUZZLE = "6x7x81xxxx5x9xxx32xxxx5xxxx2x6xxxxx3x74xxx95x8xxxxx7x4xxxx1xxxx94xxx2x7xxxx74x2x8";
        const string VALID_FINISHED_PUZZLE = "627381495451967832389254617296475183174823956835196724762518349948632571513749268";
        const string INVALID_FINISHED_PUZZLE = "627361495451967832389254617296475183174823956835196724762518349948632571513749268";
        const string INVALID_ROW_STARTING_PUZZLE = "6x7x61xxxx5x9xxx32xxxx5xxxx2x6xxxxx3x74xxx95x8xxxxx7x4xxxx1xxxx94xxx2x7xxxx74x2x8"; //Duplicate 6's in Row 1, no other invalid points
        const string INVALID_COLUMN_STARTING_PUZZLE = "6x7x81xxxx5x9xxx32xxxx5xxxx2x7xxxxx3x74xxx95x8xxxxx7x4xxxx1xxxx94xxx2x7xxxx74x2x8"; //Duplicate 7's in Column 3, no other invalid points
        const string INVALID_BLOCK_STARTING_PUZZLE = "6x7x81xxxx6x9xxx32xxxx5xxxx2x6xxxxx3x74xxx95x8xxxxx7x4xxxx1xxxx94xxx2x7xxxx74x2x8"; //Duplicate 6's in Block 1, no other invalid points


        #region InitializerTests
        [TestMethod, TestCategory("Initalizers")]
        public void NoPuzzleTest()
        {
            var noTest = new Puzzle();
            Assert.IsNotNull(noTest.Grid);
            Assert.IsNotNull(noTest.Row);
            Assert.IsNotNull(noTest.Column);
            Assert.IsNotNull(noTest.Block);
        }

        [TestMethod, TestCategory("Initalizers")]        
        public void EmptyPuzzleTest()
        {
            var emptyTest = new Puzzle("");
            Assert.IsNull(emptyTest.Grid);
            Assert.IsNull(emptyTest.Row);
            Assert.IsNull(emptyTest.Column);
            Assert.IsNull(emptyTest.Block);
            Assert.AreEqual("Invalid Starting Puzzle String", emptyTest.Status);
        }

        [TestMethod, TestCategory("Initalizers")]
        public void ShortPuzzleTest()
        {
            var shortTest = new Puzzle("123456789");
            Assert.IsNull(shortTest.Grid);
            Assert.IsNull(shortTest.Row);
            Assert.IsNull(shortTest.Column);
            Assert.IsNull(shortTest.Block);
            Assert.AreEqual("Invalid Starting Puzzle String", shortTest.Status);
        }

        [TestMethod, TestCategory("Initalizers")]
        public void LongPuzzleTest()
        {
            var longPuzzle = new StringBuilder();
            while (longPuzzle.Length < 82)
            {
                longPuzzle.Append("1");
            }
            var longTest = new Puzzle(longPuzzle.ToString());
            Assert.IsNull(longTest.Grid);
            Assert.IsNull(longTest.Grid);
            Assert.IsNull(longTest.Row);
            Assert.IsNull(longTest.Column);
            Assert.IsNull(longTest.Block);
            Assert.AreEqual("Invalid Starting Puzzle String", longTest.Status);
        }

        [TestMethod, TestCategory("Initalizers")]
        public void CorrectPuzzleTest()
        {
            var correctTest = new Puzzle(VALID_STARTING_PUZZLE);
            Assert.IsNotNull(correctTest, "Puzzle not created");
            Assert.AreEqual(9, correctTest.Row.Count, "Row NodeGroup failed");
            Assert.AreEqual(9, correctTest.Column.Count, "Column NodeGroup failed");
            Assert.AreEqual(9, correctTest.Block.Count, "Block NodeGroup failed");
            Assert.AreEqual("", correctTest.Status);
        }
        #endregion

        #region MethodTests
        [TestMethod, TestCategory("Methods")]
        public void SetupPuzzleTest()
        {
            Puzzle setupTest = new Puzzle();
            setupTest.SetupPuzzle();
            Assert.AreEqual(81, setupTest.Grid.Count, "Not enough nodes in grid");
        }

        [TestMethod, TestCategory("Methods")]
        public void CalculateBlockTest()
        {
            // Not Testing every cell, but there in each block: a one upper corner, center, and one lower corner
            Puzzle calcBlockTest = new Puzzle();
            Assert.AreEqual(1, calcBlockTest.CalculateBlock(0, 0), "Coord [0, 0, 1] Failed");
            Assert.AreEqual(1, calcBlockTest.CalculateBlock(1, 1), "Coord [1, 1, 1] Failed");
            Assert.AreEqual(1, calcBlockTest.CalculateBlock(2, 2), "Coord [2, 2, 1] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(3, 3), "Coord [3, 3, 5] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(4, 4), "Coord [4, 4, 5] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(5, 5), "Coord [5, 5, 5] Failed");
            Assert.AreEqual(9, calcBlockTest.CalculateBlock(6, 6), "Coord [6, 6, 9] Failed");
            Assert.AreEqual(9, calcBlockTest.CalculateBlock(7, 7), "Coord [7, 7, 9] Failed");
            Assert.AreEqual(9, calcBlockTest.CalculateBlock(8, 8), "Coord [8, 8, 9] Failed");
            Assert.AreEqual(3, calcBlockTest.CalculateBlock(8, 0), "Coord [8, 0, 3] Failed");
            Assert.AreEqual(3, calcBlockTest.CalculateBlock(7, 1), "Coord [7, 1, 3] Failed");
            Assert.AreEqual(3, calcBlockTest.CalculateBlock(6, 2), "Coord [6, 2, 3] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(5, 3), "Coord [5, 3, 5] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(4, 4), "Coord [4, 4, 5] Failed");
            Assert.AreEqual(5, calcBlockTest.CalculateBlock(3, 5), "Coord [3, 5, 5] Failed");
            Assert.AreEqual(7, calcBlockTest.CalculateBlock(2, 6), "Coord [2, 6, 7] Failed");
            Assert.AreEqual(7, calcBlockTest.CalculateBlock(1, 7), "Coord [1, 7, 7] Failed");
            Assert.AreEqual(7, calcBlockTest.CalculateBlock(0, 8), "Coord [0, 8, 7] Failed");
            Assert.AreEqual(2, calcBlockTest.CalculateBlock(3, 0), "Coord [3, 0, 2] Failed");
            Assert.AreEqual(2, calcBlockTest.CalculateBlock(4, 1), "Coord [4, 1, 2] Failed");
            Assert.AreEqual(2, calcBlockTest.CalculateBlock(5, 2), "Coord [5, 2, 2] Failed");
            Assert.AreEqual(8, calcBlockTest.CalculateBlock(3, 6), "Coord [3, 6, 8] Failed");
            Assert.AreEqual(8, calcBlockTest.CalculateBlock(4, 7), "Coord [4, 7, 8] Failed");
            Assert.AreEqual(8, calcBlockTest.CalculateBlock(5, 8), "Coord [5, 8, 8] Failed");

        }

        [TestMethod, TestCategory("Methods")]
        public void InitializeValuesTest()
        {
            Puzzle initValueTest = new Puzzle();
            initValueTest.SetupPuzzle();
            initValueTest.InitializeValues(VALID_STARTING_PUZZLE);
            Assert.AreEqual(6, initValueTest.Grid[0].Value, "[1,1] not set correctly");
            Assert.AreEqual(9, initValueTest.Grid[12].Value, "[4,2] not set correctly");
            Assert.AreEqual(2, initValueTest.Grid[17].Value, "[9,2] not set correctly");
            Assert.AreEqual(8, initValueTest.Grid[45].Value, "[1,6] not set correctly");
            Assert.AreEqual(0, initValueTest.Grid[40].Value, "[5,5] not set correctly");
            Assert.AreEqual(7, initValueTest.Grid[51].Value, "[7,6] not set correctly");
            Assert.AreEqual(9, initValueTest.Grid[63].Value, "[1,8] not set correctly");
            Assert.AreEqual(1, initValueTest.Grid[58].Value, "[5,7] not set correctly");
            Assert.AreEqual(8, initValueTest.Grid[80].Value, "[9,9] not set correctly");
        }

        [TestMethod, TestCategory("Methods")]
        public void ValidatePuzzleTest()
        {
            Puzzle validStartingPuzzle = new Puzzle(VALID_STARTING_PUZZLE);
            validStartingPuzzle.ValidatePuzzle();
            Assert.IsTrue(validStartingPuzzle.Valid);

            Puzzle validFinishedPuzzle = new Puzzle(VALID_STARTING_PUZZLE);
            validFinishedPuzzle.ValidatePuzzle();
            Assert.IsTrue(validFinishedPuzzle.Valid);

            Puzzle invalidColumnPuzzle = new Puzzle(INVALID_COLUMN_STARTING_PUZZLE);
            invalidColumnPuzzle.ValidatePuzzle();
            Assert.IsFalse(invalidColumnPuzzle.Valid);

            Puzzle invalidRowPuzzle = new Puzzle(INVALID_ROW_STARTING_PUZZLE);
            invalidRowPuzzle.ValidatePuzzle();
            Assert.IsFalse(invalidRowPuzzle.Valid);

            Puzzle invalidBlockPuzzle = new Puzzle(INVALID_BLOCK_STARTING_PUZZLE);
            invalidBlockPuzzle.ValidatePuzzle();
            Assert.IsFalse(invalidBlockPuzzle.Valid);

            Puzzle invalidFinishedPuzzle = new Puzzle(INVALID_FINISHED_PUZZLE);
            invalidFinishedPuzzle.ValidatePuzzle();
            Assert.IsFalse(invalidFinishedPuzzle.Valid);


        }
        #endregion



    }
}

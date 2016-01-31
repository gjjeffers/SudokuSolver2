using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver2.Models
{
    public class Puzzle
    {
        public List<Node> Grid;
        public string Status = "";
        public bool Valid=false;
        public bool Complete = false;
        public List<NodeGroup> Row;
        public List<NodeGroup> Column;
        public List<NodeGroup> Block;

        public Puzzle()
        {
            Grid = new List<Node>();
            Row = new List<NodeGroup>();
            Column = new List<NodeGroup>();
            Block = new List<NodeGroup>();
        }

        public Puzzle(string startingPuzzle) {
            if(startingPuzzle.Length != 81) {
                Status = "Invalid Starting Puzzle String";
                return;
            }
            Grid = new List<Node>();
            Row = new List<NodeGroup>();
            Column = new List<NodeGroup>();
            Block = new List<NodeGroup>();
            SetupPuzzle();
            for(int i = 1; i < 10; i++)
            {
                Row.Add(new NodeGroup(Grid.Where(n => n.Row == i).ToList()));
                Column.Add(new NodeGroup(Grid.Where(n => n.Column == i).ToList()));
                Block.Add(new NodeGroup(Grid.Where(n => n.Block == i).ToList()));
            }
            InitializeValues(startingPuzzle);
        }

        public void SetupPuzzle()
        {
            int column = 0;
            int row = 0;
            int block = 0;
            int value = 0;
            for(int i = 0; i < 81; i++)
            {
                column = (i % 9);
                row = (i / 9);
                block = CalculateBlock(column, row);                
                // adding 1 to column and row gives a result grid starting at [1,1]. 
                // couldn't do this earlier without effecting the block calculation
                Grid.Add(new Node() { Column = column + 1, Row = row+1, Block = block, Value = value });
            }
        }

        public int CalculateBlock(int column, int row)
        {
            // assuming a block structure as follows:
            // 1 | 2 | 3
            // 4 | 5 | 6
            // 7 | 8 | 9
            int block = 0;
            // substracting one makes the integer division a bit easier. 
            block = ((column) / 3) + 1; // yields 1,2, or 3 depending on the column
            block += 3 * (row / 3); // adds the appropirate value to shift the block number down to the correct block row (see above structure)
            return block;
        }

        public void InitializeValues(string puzzle)
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                if(puzzle[i] >= '1' && puzzle[i] <= '9')
                {
                    Grid[i].SetValue(int.Parse(puzzle[i].ToString()));
                }
            }
        }

        public void ValidatePuzzle()
        {

        }       

        public void Start(){}
    }
}
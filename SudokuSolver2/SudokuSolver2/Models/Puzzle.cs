using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SudokuSolver2.Models
{
    public class Puzzle: INotifyPropertyChanged, ICloneable
    {
        public List<Node> Grid;
        public string Status = "";
        public bool Complete = false;
        public List<NodeGroup> Sections;
        public int ActivityTracker = 0;
        public bool Initializing = false;
        public bool Solving = true;
        public bool UpdatedOnPass = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public Puzzle()
        {
            /* main object. contains the grid of nodes in a list. contains all NodeGroups in a list.
               validates and determines completeness of puzzles.
               solves puzzles through deductive logic contained in NodeGroup until no singluar values remain.
               if deductive solving stops without a completed puzzle, solving "guesses" using the first possibility 
               of the first available node, if that node fails to resolve into a valid and complete puzzle, the possibility
               is removed and the "guess" attempts the same with the next available possibility.
               if no solution can be found, the last good state of the puzzle is returned with 0 at indeterminate positions
            */
            Grid = new List<Node>();
            Sections = new List<NodeGroup>();
            SetupPuzzle();
            // add each row, column, and block to Sections and functions to listen for activity
            for (int i = 1; i < 10; i++)
            {
                NodeGroup newRow = new NodeGroup(Grid.Where(n => n.Row == i).ToList());
                newRow.PropertyChanged += TrackActivity;
                Sections.Add(newRow);
                NodeGroup newColumn = new NodeGroup(Grid.Where(n => n.Column == i).ToList());
                newColumn.PropertyChanged += TrackActivity;
                Sections.Add(newColumn);
                NodeGroup newBlock = new NodeGroup(Grid.Where(n => n.Block == i).ToList());
                newBlock.PropertyChanged += TrackActivity;
                Sections.Add(newBlock);
            }
        }

        public Puzzle(string startingPuzzle)
        {
            // any starting string not 81 characters long is not a valid puzzle
            if (startingPuzzle.Length != 81)
            {
                Status = "Invalid Starting Puzzle String";
                return;
            }
            Grid = new List<Node>();
            Sections = new List<NodeGroup>();
            SetupPuzzle();
            for(int i = 1; i < 10; i++)
            {
                NodeGroup newRow = new NodeGroup(Grid.Where(n => n.Row == i).ToList());
                newRow.PropertyChanged += TrackActivity;
                Sections.Add(newRow);
                NodeGroup newColumn = new NodeGroup(Grid.Where(n => n.Column == i).ToList());
                newColumn.PropertyChanged += TrackActivity;
                Sections.Add(newColumn);
                NodeGroup newBlock = new NodeGroup(Grid.Where(n => n.Block == i).ToList());
                newBlock.PropertyChanged += TrackActivity;
                Sections.Add(newBlock);
            }
            InitializeValues(startingPuzzle);
        }

        public void SetupPuzzle()
        {
            // create initial grid. nodes are assigned locations as they are inserted into the list
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
            block = ((column) / 3) + 1; // yields 1,2, or 3 depending on the column
            block += 3 * (row / 3); // adds the appropirate value to shift the block number down to the correct block row (see above structure)
            return block;
        }

        public void InitializeValues(string puzzle)
        {
            // read initial string and assign values to appropriate nodes
            // as values are being assigned possibilites are adjusted accordingly by the NodeGroups
            Initializing = true;
            for (int i = 0; i < puzzle.Length; i++)
            {
                if(puzzle[i] >= '1' && puzzle[i] <= '9')
                {
                    Grid[i].SetValue(int.Parse(puzzle[i].ToString()));
                }
            }
            Initializing = false;
        }

        public void Solve()
        {
            // solve the puzzle using deductive logic and loop until there are no longer any changes to node values
            while (UpdatedOnPass)
            {
                UpdatedOnPass = false;
                foreach (NodeGroup ng in Sections)
                {
                    ng.Solve();
                }
            }
            // if basic solver has completed but the puzzle is not completely filled in and the puzzle is still valid 
            // a condition has been reached where there are no single possibilities left and a guess needs to be made
            if (IsFinished() && Grid.Count(zeroValueCount => zeroValueCount.Value == 0) > 0)
            {
                Guess();
            }
        }

        public bool IsValid()
        {
            // check for validity by looking for nodes without values and no remaining possibilities
            foreach (Node n in Grid)
            {
                if(n.Possibilities.Count == 0 && n.Value == 0){
                    return false;
                }
            }

            // check for more than one value being set in the NodeGroup
            foreach(NodeGroup s in Sections)
            {
                for (int i = 1; i < 10; i++)
                {
                    if (s.Nodes.Count(valueCount => valueCount.Value == i) > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }       

        public bool IsFinished()
        {
            // check that the NodeGroups are no longer doing work 
            // if still reading the initial string, the puzzle is not finished
            // (NodeGroups can respond faster than the next value can be read in some case)
            if (Initializing) { return false; };
            Complete = ActivityTracker <= 0;
            return IsValid() && Complete;
        }

        public void TrackActivity(object sender, PropertyChangedEventArgs e)
        {
            // listener for activity in NodeGroups. if activity is detected, Solve() will need to
            // go through another pass as singular possibilities may have been created.
            // ActivityTracker keeps count of how many NodeGroups are currently doing work.
            UpdatedOnPass = true;
            NodeGroup origin = sender as NodeGroup;
            if (origin.Busy)
            {
                ActivityTracker++;
            }
            else
            {
                ActivityTracker--;
            }
        }

        public void Guess()
        {
            // it has been determined that the solution to the puzzle can not be reached through just deductive logic
            // and now Solve needs to make a calculated guess using the possibilities of the first unknown node.
            // call Solve recursively until the puzzle is determined to be unsolvable or a solution has been reached.
            Puzzle guessPuzzle = this.Clone() as Puzzle; // clone existing puzzle
            Node guessNode = guessPuzzle.Grid.First(emptyNode => emptyNode.Value == 0); // find first node without a value
            int guessValue = guessNode.Possibilities[0]; // get first possibility of node
            guessNode.SetValue(guessValue); // set value of clone's first unknown node
            guessPuzzle.Solve(); // attempt to solve clone puzzle
            Node fillNode = Grid.First(emptyNode => emptyNode.Value == 0); // get first node of existing puzzle without a value (corresponds to clone's first unknown)
            if (guessPuzzle.IsFinished()) // if clone puzzle has found a valid solution, set that value on the existing puzzle
            {
                fillNode.SetValue(guessValue);
            }
            else // if clone puzzle has reached the conclusion that the guess value resulted in an unsolvable puzzle, remove the possibility from the existing node
            {
                fillNode.Possibilities.Remove(guessValue);
            }
            UpdatedOnPass = true; //changes have occurred to the grid and solve will need to make a new pass
            Solve();
        }

        public override string ToString()
        {
            // returns a string representation of the current set of values.
            // used for testing
            StringBuilder puzzleString = new StringBuilder();
            foreach(Node n in Grid)
            {
                puzzleString.Append(n.Value.ToString());
            }
            return puzzleString.ToString();
        }

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public object Clone()
        {
            var clonedPuzzle = new Puzzle();
            clonedPuzzle.Grid = new List<Node>();
            foreach(Node n in Grid)
            {
                clonedPuzzle.Grid.Add((Node)n.Clone());
            }
            for (int i = 1; i < 10; i++)
            {
                NodeGroup newRow = new NodeGroup(clonedPuzzle.Grid.Where(n => n.Row == i).ToList());
                newRow.PropertyChanged += TrackActivity;
                clonedPuzzle.Sections.Add(newRow);
                NodeGroup newColumn = new NodeGroup(clonedPuzzle.Grid.Where(n => n.Column == i).ToList());
                newColumn.PropertyChanged += TrackActivity;
                clonedPuzzle.Sections.Add(newColumn);
                NodeGroup newBlock = new NodeGroup(clonedPuzzle.Grid.Where(n => n.Block == i).ToList());
                newBlock.PropertyChanged += TrackActivity;
                clonedPuzzle.Sections.Add(newBlock);
            }

            return clonedPuzzle;
        }
    }
}
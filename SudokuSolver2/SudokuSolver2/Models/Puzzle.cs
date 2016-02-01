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
            Grid = new List<Node>();
            Sections = new List<NodeGroup>();
            SetupPuzzle();
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
            while (UpdatedOnPass)
            {
                UpdatedOnPass = false;
                foreach (NodeGroup ng in Sections)
                {
                    ng.Solve();
                }
            }
            if (IsFinished() && Grid.Count(zeroValueCount => zeroValueCount.Value == 0) > 0)
            {
                Guess();
            }
        }

        public bool IsValid()
        {
            foreach (Node n in Grid)
            {
                if(n.Possibilities.Count == 0 && n.Value == 0){
                    return false;
                }
            }

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
            if (Initializing) { return false; };
            Complete = ActivityTracker <= 0;
            return IsValid() && Complete;
        }

        public void TrackActivity(object sender, PropertyChangedEventArgs e)
        {
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
            Puzzle guessPuzzle = this.Clone() as Puzzle;
            Node guessNode = guessPuzzle.Grid.First(emptyNode => emptyNode.Value == 0);
            int guessValue = guessNode.Possibilities[0];
            guessNode.SetValue(guessValue);
            guessPuzzle.Solve();
            Node fillNode = Grid.First(emptyNode => emptyNode.Value == 0);                    
            if (guessPuzzle.IsFinished())
            {
                fillNode.SetValue(guessValue);
            }
            else
            {
                fillNode.Possibilities.Remove(guessValue);
            }
            UpdatedOnPass = true;
            Solve();
        }

        public override string ToString()
        {
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
                clonedPuzzle.Grid.Add(n.Clone() as Node);
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
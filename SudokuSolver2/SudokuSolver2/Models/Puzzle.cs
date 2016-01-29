using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver2.Models
{
    public class Puzzle
    {
        public Puzzle(string s) {
            if(s.Length != 81) { throw new ArgumentException("Invalid Starting Puzzle String"); }
        }

        public void Start(){}
    }
}
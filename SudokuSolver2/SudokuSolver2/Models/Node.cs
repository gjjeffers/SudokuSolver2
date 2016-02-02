using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SudokuSolver2.Models
{
    public class Node: INotifyPropertyChanged, ICloneable
    {
        // basic building block of puzzle. contains list of possibilities, its position in the puzzle, and value
        public List<int> Possibilities = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9};
        public int Value = 0;
        public int Row = 0;
        public int Column = 0;
        public int Block = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue(int v)
        {
            // set the value and remove all possibilities so they are not longer checked
            Value = v;
            Possibilities = new List<int>();
            //notify listeners (NodeGroup) that the value of its member node has been set
            NotifyPropertyChanged("Value");
        }

        protected void NotifyPropertyChanged(String info)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public object Clone()
        {
            var newNode = new Node() { Value = this.Value, Row = this.Row, Column = this.Column, Block = this.Block };
            newNode.Possibilities = new List<int>();
            foreach(int i in Possibilities)
            {
                newNode.Possibilities.Add(i);
            }
            return newNode;
        }
    }
}
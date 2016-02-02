using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SudokuSolver2.Models
{
    public class NodeGroup: INotifyPropertyChanged
    {
        // collection for nodes representing row, column, and block. manages possibilities among member nodes as values become set. 
        // examines member nodes for singular possibilities, if found sets corresponding node's value
        public List<Node> Nodes;
        private bool _busy = false;
        public bool Busy  //flag to notify listeners (Puzzle) that activity is currently happening in the NodeGroup
        {
            get { return _busy; }
            set
            {
                _busy = value;
                NotifyPropertyChanged("Busy");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NodeGroup(List<Node> NodePool)
        {
            Nodes = NodePool;
            // assign delegate to listen for values being set on member nodes and remove established values as possibilities from other member nodes
            foreach (Node n in Nodes) 
            {
                n.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    Busy = true;
                    foreach (Node node in Nodes)
                    {
                        node.Possibilities.Remove(((Node)sender).Value);                        
                    }
                    Busy = false;
                };
            }
        }

        public int PossibilityCount(int poss)
        {
            // finds the number of occurances of poss in member nodes
            // a return of 1 indicates a singular value that can be set
            int _possCount = 0;
            foreach(Node n in Nodes)
            {
                if (n.Possibilities.Contains(poss))
                {
                    _possCount++;
                }
            }
            return _possCount;
        }

        public void Solve()
        {
            // examines member nodes for singular values and sets the value if found

            // examine each member node for only having one possibility 
            foreach(Node x in Nodes)
            {
                if(x.Possibilities.Count == 1)
                {
                    x.SetValue(x.Possibilities[0]);
                }
            }

            // examine all member nodes for a possibility count of 1
            for(int i = 1; i < 10; i++)
            {
                if(PossibilityCount(i) == 1)
                {
                    foreach(Node n in Nodes)
                    {
                        if (n.Possibilities.Contains(i))
                        {
                            n.SetValue(i);
                        }
                    }
                }
            }
        }

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


    }
}
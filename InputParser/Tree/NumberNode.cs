using System;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        class NumberNode : INode
        {
            private double Number { get; }
            public bool Contains()
            {
                return false;
            }
            public double Evaluate()
            {
                return Number;
            }

            public string Symbolic()
            {
                return Number.ToString();
            }

            public NumberNode(double value)
            {
                Number = value;
            }

            public INode Left
            {
                get { return null; }
                set { throw new NotSupportedException(); }
            }


            public INode Right
            {
                get { return null; }
                set { throw new NotSupportedException(); }
            }

            public bool Contains(string a)
            {
                return false;
            }
        }

    }
}
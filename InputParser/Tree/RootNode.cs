using System;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        public class RootNode : INode
        {
            public double Evaluate()
            {
                return Left.Evaluate();
            }

            public string Symbolic()
            {
                return "=";
            }
            public INode Left { get; set; }
            public INode Right { get; set; }

            public bool Contains(string a)
            {
                throw new NotSupportedException();
            }
        }



    }
}
using System;
using System.Collections.Generic;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        class OperatorNode : INode
        {
            private static Dictionary<string, Func<double, double, double>> Functions =
                new Dictionary<string, Func<double, double, double>>
                {
                    { "+", (x,y)=> x+y},
                    { "-", (x,y)=> x-y},
                    { "~", (x,y)=> -x},
                    { "*", (x,y)=> x*y},
                    { "/", (x,y)=> x/y},
                    { "^", Math.Pow},
                    { "&", (x,y) => Math.Pow(x,1/y)},
                };

            private static Dictionary<string, string> OppositeOperator =
                new Dictionary<string, string>
                {
                    {"+","-" },
                    {"-","+" },
                    {"*","/" },
                    {"/","*" },
                    {"^","&" },
                    {"&","^" },
                };
            public OperatorNode(string op)
            {
                SetOperator(op);
            }
            private void SetOperator(string op)
            {

                Function = Functions[op];
                Operator = op;

            }
            private string Operator { get; set; }
            private Func<double, double, double> Function { get; set; }

            public double Evaluate()
            {
                double right = Right?.Evaluate() ?? 0;
                return Function(Left.Evaluate(), right);
            }

            public string Symbolic()
            {
                return Operator;
            }

            public bool Contains(string a)
            {
                bool left = Left.Contains(a);
                bool right = false;
                if (Right != null)
                    right = Right.Contains(a);
                return left || right;
            }
            public INode Left { get; set; }
            public INode Right { get; set; }

            public void SwapOperator()
            {
                SetOperator(OppositeOperator[Operator]); // TODO : opposite operator!
            }
        }



    }
}
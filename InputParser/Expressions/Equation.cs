using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputParser.Tree;

namespace InputParser.Expressions
{
    public class Equation
    {
        public string Variable { get; }
        private ExpressionTree Tree { get; set; }
        public Equation(string data)
        {
            Tree = new ExpressionTree(data);
        }
        private static readonly Dictionary<string, double> Empty = new Dictionary<string, double>();
        public double Evaluate(Dictionary<string, double> dict = null)
        {
            if (dict == null)
                dict = Empty;
            return Tree.Evaluate(dict);
        }

        public void TransformFor(string variable)
        {
            Tree.RotateFor(variable);
        }

        public string Symbolic()
        {
            return Tree.Symbolic();
        }
    }
}

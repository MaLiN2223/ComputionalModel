using System;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        public class VariableNode : INode
        {
            public VariableNode(string variable)
            {
                Variable = variable;
            }
            private string Variable { get; }
            private double? _value;

            public string Symbolic()
            {
                return Variable;
            }
            private double Value
            {
                get
                {
                    if (_value == null)
                        throw new NotSupportedException($"Value for {Variable} is not set");
                    var tmp = _value.Value;
                    _value = null;
                    return tmp;
                }
                set { _value = value; }
            }

            public void SetTemporaryValue(double value)
            {
                Value = value;
            }

            public double Evaluate()
            {
                return Value;
            }

            public bool Contains(string a)
            {
                return Variable == a;
            }

            public INode Left
            {
                get { return null; }
                set { throw new NotSupportedException();}
            }

            public INode Right
            {
                get { return null; }
                set { throw new NotSupportedException(); }
            }
        }



    }
}
using System;
using System.Collections.Generic;
using InputParser.Tokens;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        class OperatorNodeVisitor
        {
            private readonly IComparer<OperatorNode> m_comparer = new OperatorPrecedenceComparer();

            class OperatorPrecedenceComparer : Comparer<OperatorNode>
            {
                public override int Compare(OperatorNode x, OperatorNode y)
                {
                    if (x == null || y == null)
                        return int.MaxValue;
                    var px = Precedence(x);
                    var py = Precedence(y);
                    if (px == py)
                    {
                        var pxA =
                            EvaluationData.OperatorData[x.Symbolic()].Associativity == Associativity.Left ? 1 : 0;
                        var pyA = EvaluationData.OperatorData[y.Symbolic()].Associativity == Associativity.Left ? 1 : 0;
                        return pxA.CompareTo(pyA);

                    }
                    return px.CompareTo(py);
                }

                private static int Precedence(OperatorNode expressionType)
                {
                    return EvaluationData.OperatorData[expressionType.Symbolic()].Priority;
                }
            }

            public string Visit(INode node)
            {
                if (node == null)
                    return null;
                string a = Visit(node, node.Left);
                string c = node.Symbolic();
                string b = Visit(node, node.Right);

                return a + c + b;
            }

            private string Visit(INode parent, INode child)
            {
                var tmpChild = child as OperatorNode;
                var tmpParent = parent as OperatorNode;
                int value = m_comparer.Compare(tmpChild, tmpParent);
                if (value < 0)
                {
                    return $"({Visit(child)})";
                }
                else
                {
                    return Visit(child);
                }
            }
        }
    }




}
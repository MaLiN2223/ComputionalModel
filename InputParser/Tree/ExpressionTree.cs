using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using InputParser.Tokens;

namespace InputParser.Tree
{
    public enum TokenType
    {
        Number, Function, Operator, Variable
    }
    public partial class ExpressionTree
    {
        private readonly List<VariableNode> Variables = new List<VariableNode>();
        public void RotateFor(string input)
        {
            var current = Root.Right; 
            bool changes = false;
            while ((current as VariableNode) == null)
            {
                var curr = (current as OperatorNode);
                if (current.Right.Contains(input))
                {
                    var tmpRootLeft = Root.Left;
                    var tmpCurrentRight = curr.Right;
                    var tmpCurrentLeft = curr.Left;
                    if (EvaluationData.OperatorData[curr.Symbolic()].Associativity == Associativity.Left)
                    { 
                        curr.Left = tmpCurrentLeft;
                        curr.Right = tmpRootLeft; 
                    }
                    else
                    {
                        curr.SwapOperator();
                        curr.Left = tmpRootLeft;
                        curr.Right = tmpCurrentLeft;
                    }
                    Root.Right = tmpCurrentRight;
                    Root.Left = curr;
                    current = Root.Right;

                    changes = true;
                }
                else if (current.Left.Contains(input))
                {
                    var tmpRootLeft = Root.Left;
                    var tmpCurrentLeft = curr.Left;
                    curr.SwapOperator();
                    curr.Left = tmpRootLeft;
                    Root.Right = tmpCurrentLeft;
                    Root.Left = curr;
                    current = Root.Right;

                    changes = true;
                }
                else
                    break;
            }
            if (changes)
            {
                var tmp = Root.Right;
                Root.Right = Root.Left;
                Root.Left = tmp;
            }
            RefreshVariables(); 
        }

        private void RefreshVariables()
        {
            Variables.Clear();
            Stack<INode> nodes = new Stack<INode>();
            nodes.Push(Root.Right);
            while (nodes.Count > 0)
            {
                var curr = nodes.Pop();
                if (curr == null)
                    continue;
                nodes.Push(curr.Left);
                nodes.Push(curr.Right);
                var mpt = curr as VariableNode;
                if (mpt != null)
                    Variables.Add(mpt);
            }
        }
        public ExpressionTree(string equation)
        {
            var splitted = equation.Split('=');

            Root = new RootNode
            {
                Left = new VariableNode(splitted[0].Trim())
            };
            FillTree(ToPostfix(Tokenizer.Tokenize(splitted[1])));

        }
        private void FillTree(List<Token> tokens)
        {
            var Stack = new Stack<INode>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Variable)
                {
                    var variable = new VariableNode(token.Value);
                    Variables.Add(variable);
                    Stack.Push(variable);
                }
                else if (token.Type == TokenType.Number)
                {
                    Stack.Push(new NumberNode(Double.Parse(token.Value)));
                }
                else if (token.Value == "~")
                {
                    var tmp = Stack.Pop();
                    var func = new OperatorNode(token.Value) { Left = tmp };
                    Stack.Push(func);
                }
                else if (token.Type == TokenType.Operator)
                {
                    var tmp1 = Stack.Pop();
                    var tmp2 = Stack.Pop();
                    var func = new OperatorNode(token.Value) { Left = tmp2, Right = tmp1 };
                    Stack.Push(func);
                }
                else
                {
                    //TODO : do not ignore ~
                    //throw new ArgumentException("Problem with token?");
                }
            }
            Root.Right = Stack.Pop();
        }
        private static List<Token> ToPostfix(List<Token> tokens)
        {
            var output = new Queue<Token>();
            var stack = new Stack<Token>();
            bool unaryMinus = false;
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number || token.Type == TokenType.Variable)
                {
                    if (unaryMinus)
                    {
                        unaryMinus = false;
                    }
                    output.Enqueue(token);
                }
                else if (token.Value == "~")
                {
                    unaryMinus = !unaryMinus;
                    stack.Push(token);
                }
                else if (token.Type == TokenType.Operator && token.Value != ")" && token.Value != "(")
                {
                    unaryMinus = false;
                    var data = EvaluationData.OperatorData[token.Value];
                    while (stack.Count > 0 && stack.Peek().Type == TokenType.Operator)
                    {
                        var tmp = stack.Peek();
                        var tmpData = EvaluationData.OperatorData[tmp.Value];
                        if ((data.Associativity == Associativity.Left && data.Priority <= tmpData.Priority)
                            ||
                            (data.Associativity == Associativity.Right && data.Priority < tmpData.Priority)
                        )
                        {
                            tmp = stack.Pop();
                            output.Enqueue(tmp);
                        }
                        else
                        {
                            break;
                        }
                    }
                    stack.Push(token);
                }
                else if (token.Value == "(")
                {
                    unaryMinus = false;
                    stack.Push(token);
                }
                else if (token.Value == ")")
                {
                    unaryMinus = false;
                    while (stack.Count > 0 && stack.Peek().Value != "(")
                    {
                        output.Enqueue(stack.Pop());
                    }
                    if (stack.Count > 0)
                    {
                        if (stack.Peek().Value != "(")
                        {
                            throw new ArgumentException("Left bracket not found");
                        }
                        stack.Pop();
                        if (stack.Count > 0 && stack.Peek().Value == "~")
                        {
                            output.Enqueue(stack.Pop());
                        }
                    }
                }
            }
            while (stack.Count > 0)
            {
                var tmp = stack.Pop();
                if (tmp.Value == ")" || tmp.Value == "(")
                    continue;
                output.Enqueue(tmp);
            }
            return output.ToList();
        }


        public double Evaluate(Dictionary<string, double> variablesValues)
        {
            foreach (var x in Variables)
            {
                x.SetTemporaryValue(variablesValues[x.Symbolic()]);
            }
            return Root.Right.Evaluate();
        }

        public string Symbolic()
        {
            var visitor = new OperatorNodeVisitor();
            var left = visitor.Visit(Root.Left);
            var right = visitor.Visit(Root.Right);
            return $"{left} = {right}";

        }
        RootNode Root { get; set; }
    }

}
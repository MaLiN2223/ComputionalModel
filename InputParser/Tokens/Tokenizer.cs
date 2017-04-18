using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputParser.Tree;

namespace InputParser.Tokens
{
    public static class Tokenizer
    {
        private static bool IsOperator(string c)
        {
            return c == "+" || c == "-" || c == "*" || c == "/" || c == "^";
        }
        enum MinusState
        {
            Normal, Negation, None
        }
        public static List<Token> Tokenize(string input)
        {
            var list = new List<Token>();
            Action<string> dumpOperand =
                c =>
                {
                    if (c != "")
                    {
                        if (char.IsNumber(c[0]))
                        {
                            list.Add(new Token(c, TokenType.Number));
                        }
                        else
                        {
                            list.Add(new Token(c, TokenType.Variable));
                        }
                    }
                };
            string lastOperand = "";
            string lastCharacter = "";
            var minusState = MinusState.Negation;
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    dumpOperand(lastOperand);
                    lastOperand = "";
                }
                else if (c == ')')
                {
                    dumpOperand(lastOperand);
                    lastOperand = "";
                    var token = new Token(")", TokenType.Operator);
                    list.Add(token);
                    minusState = MinusState.Normal;
                }
                else if (c == '(')
                {
                    if (lastOperand != "")
                    {
                        throw new ArgumentException($"Number cannot be so close to (, {lastOperand}");
                    }
                    minusState = MinusState.Negation;
                    var token = new Token("(", TokenType.Operator);
                    list.Add(token);
                }
                else if (IsOperator(c.ToString()))
                {
                    if (c == '-' && (minusState == MinusState.Negation))
                    {
                        dumpOperand(lastOperand);
                        lastOperand = "";
                        var token = new Token("~", TokenType.Operator);
                        list.Add(token);
                    }
                    else if (((c == '-') || c == '+') && lastOperand.Length > 0 && lastOperand[lastOperand.Length - 1] == 'e')
                    {
                        lastOperand += c;
                    }
                    else
                    {
                        dumpOperand(lastOperand);
                        lastOperand = "";
                        var token = new Token(c, TokenType.Operator);
                        list.Add(token);
                        minusState = MinusState.Negation;
                    }
                }
                else if (char.IsNumber(c) || char.IsLetter(c))
                {
                    lastOperand += c;
                    minusState = MinusState.Normal;
                }
                else
                {
                    throw new ArgumentException($"Wrong input {c}");
                }
            }
            if (lastOperand != "")
                dumpOperand(lastOperand);

            return list;
        }

    }
}

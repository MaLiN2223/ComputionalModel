﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputParser.Tokens
{
    public enum Associativity
    {
        Left,
        Right
    }
    public struct OperatorData
    {
        public OperatorData(int priority, Associativity associativity)
        {
            Priority = priority;
            Associativity = associativity;
        }
        public int Priority { get; set; }
        public Associativity Associativity { get; set; }
    }
    static class EvaluationData
    {
        public static readonly Dictionary<string, Func<double, double, double>> OperatorsDictionary = new Dictionary
            <string, Func<double, double, double>>
            {
                {"+", (d0, d1) => d0 + d1},
                {"-", (d0, d1) => d0 - d1},
                {"*", (d0, d1) => d0*d1},
                {"/", (d0, d1) => d0/d1},
                {"^", Math.Pow},
                {"~",(d0,d1)=>-d0 }

            };

        public static readonly Dictionary<string, Func<double, double>> FunctionsDictionary = new Dictionary
            <string, Func<double, double>>
            {
                {"log",Math.Log},
                {"ln",Math.Log10},
                {"exp",Math.Exp},
                {"sqrt", Math.Sqrt},
                {"abs",  Math.Abs},
                {"Abs",  Math.Abs},
                {"atan", Math.Atan},
                {"acos",  Math.Acos} ,
                {"asin", Math.Asin},
                {"sinh",  Math.Sinh},
                {"cosh",  Math.Cosh},
                {"tanh",  Math.Tanh},
                {"tan", Math.Tan},
                {"sin", Math.Sin},
                {"cos", Math.Cos},
                {"neg", d=>-d},
                
            };

        public static readonly Dictionary<string, OperatorData> OperatorData = new Dictionary<string, OperatorData>
        {
            { "~", new OperatorData(4,Associativity.Right)},
            { "+", new OperatorData(1,Associativity.Right)},
            { "-", new OperatorData(1,Associativity.Left)},
            { "*", new OperatorData(2,Associativity.Right)},
            { "/", new OperatorData(2,Associativity.Left)},
            { "^", new OperatorData(5,Associativity.Right)},
            { "&", new OperatorData(5,Associativity.Right)},
            { ")", new OperatorData(1,Associativity.Left)},
            { "(", new OperatorData(0,Associativity.Right)},
        };
    }
}

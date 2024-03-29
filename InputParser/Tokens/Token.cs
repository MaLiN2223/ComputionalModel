﻿namespace InputParser.Tree
{
    public class Token
    { 
        public TokenType Type { get; }
        public string Value { get; set; }

        public Token(char value, TokenType type) : this(value.ToString(), type)
        {

        }
        public Token(string value, TokenType type)
        {
            Type = type;
            Value = value.ToLower();
        }
    }
}
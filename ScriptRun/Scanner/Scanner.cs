/*
 * Copyright 2022 heathk1iff@outlook.com
 * Licensed under the Apache License, Version 2.0
*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScriptRun.Scan
{
    public class Scanner
    {
        public static char Symbol_Space = ' ';
        public static char Symbol_Equal = '=';
        public static char Symbol_BeginBracket = '(';
        public static char Symbol_EndBracket = ')';
        public static char Symbol_BeginBlock = '{';
        public static char Symbol_EndBlock = '}';
        public static char Symbol_Semicolon = ';';
        public static char Symbol_Plus = '+';
        public static char Symbol_Minus = '-';
        public static char Symbol_Comma = ',';
        public static char Symbol_Divide = '/';
        public static char Symbol_Multiply = '*';

        StreamReader _reader;

        (TokenType Type, string Pattern)[] _tokenTypes = {
              (Type: TokenType.Id, Pattern: "[A-Za-z]([A-Za-z0-9_]+)?"),
              (Type: TokenType.Number, Pattern: @"\-?\d+(\.\d+)?"),
              (Type: TokenType.Text, Pattern: @"'.+'"),
              (Type: TokenType.Assign, Pattern: "="),
              (Type: TokenType.BeginBracket, Pattern: @"\("),
              (Type: TokenType.EndBracket, Pattern: @"\)"),
              (Type: TokenType.Comma, Pattern: $","),
              (Type: TokenType.Semicolon, Pattern: ";"),
              (Type: TokenType.BeginBlock, Pattern: "{"),
              (Type: TokenType.EndBlock, Pattern: "}"),
              (Type: TokenType.Plus, Pattern: @"\+"),
              (Type: TokenType.Minus, Pattern: @"\-"),
              (Type: TokenType.Divide, Pattern: @"/"),
              (Type: TokenType.Multiply, Pattern: @"\*")
        };

        char[] _reservedSymbols = { Scanner.Symbol_Equal, Scanner.Symbol_BeginBracket, Scanner.Symbol_Minus, Scanner.Symbol_Divide,
                                    Scanner.Symbol_EndBracket, Scanner.Symbol_Semicolon, Scanner.Symbol_Plus,
                                    Scanner.Symbol_Comma, Scanner.Symbol_BeginBlock, Scanner.Symbol_EndBlock, Scanner.Symbol_Multiply};
        string[] _reservedWords = { };


        public Scanner(Stream stream)
        {
            stream.Position = 0;
            _reader = new StreamReader(stream);
        }

        public Token GetToken()
        {
            int nchr = _reader.Peek();

            while (((nchr = _reader.Peek()) == Symbol_Space) ||
                (nchr == '\r') || (nchr == '\n')) 
            {
                _reader.Read();
            };

            if (nchr < 0)
                return null;


            bool isTextBlock = false;

            StringBuilder token = new StringBuilder();
            while (((nchr = _reader.Peek()) > 0) && (nchr != Symbol_Space) && (nchr != '\r') && (nchr != '\n'))
            {

                if ((!isTextBlock) && (Array.IndexOf(_reservedSymbols, (char)nchr) >= 0))
                {
                    if (token.Length == 0)
                    {
                        token.Append((char)_reader.Read());
                        
                        if (((char)nchr == Symbol_Minus) && (char.IsDigit((char)_reader.Peek())))
                            continue;  
                    }
                    break;
                }
                
                if ((char)nchr == '\'')
                {
                    isTextBlock = (token.Length == 0);
                }

                token.Append((char)_reader.Read());
            };

            string value = token.ToString();
            var tokenType = _tokenTypes.FirstOrDefault(exp => Regex.IsMatch(value, '^' + exp.Pattern + '$'));

            if (tokenType == default((TokenType, string)))
                return new Token(TokenType.Undefined, value);

            if ((tokenType.Type == TokenType.Id) && (_reservedWords.Any(f => f == value)))
                return new Token(TokenType.ReservedWord, value);

            return new Token(tokenType.Type, value);

        }
    }
}

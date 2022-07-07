using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            _source = source;
        }

        /// <summary>
        /// Fills list with generated tokens before appending final 'end of file' token.
        /// </summary>
        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, line));
            return _tokens;
        }

        private Boolean IsAtEnd()
        {
            return current >= _source.Length;
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                default: Lox.Error(line, "Unexpected character."); break;
            }
        }

        private char Advance()
        {
            return _source[current++]; 
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(start, current);
            _tokens.Add(new Token(type, text, literal, line));
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }
    }
}
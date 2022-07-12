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

        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };

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
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.EQUAL); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if (Match('/')) 
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    } 
                    else 
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ': 
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;
                case '\n': line++; break;
                case '"': String(); break;
                default: 
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Lox.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        // For identifiers.
        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(start, current);

            if (!keywords.TryGetValue(text, out TokenType type))
            {
                type = TokenType.IDENTIFIER;
            }

            AddToken(type);
        }

        // For numbers.
        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            // Look for a fractional part.
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER, Double.Parse(_source.Substring(start, current)));
        }

        // For string literals.
        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            string value = _source.Substring(start + 1, current - 1);
            AddToken(TokenType.STRING, value);
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

        /// <summary>
        /// Determines operator type based on the token following the current token.
        /// </summary>
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[current] != expected) return false;

            current++;
            return true;
        }

        /// <summary>
        /// Looks ahead one character.
        /// </summary>
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[current];
        }

        /// <summary>
        /// Looks ahead a character past the decimal point.
        /// </summary>
        private char PeekNext()
        {
            if (current + 1 >= _source.Length) return '\0';
            return _source[current + 1];
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c == '_');
        }

        private bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');   
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAtEnd()
        {
            return current >= _source.Length;
        }
    }
}
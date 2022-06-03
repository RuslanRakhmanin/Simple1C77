using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    public class Lexer
    {
        string _text;
        uint _pos = 0;
        uint _line = 1;
        uint _colon = 1;
        private char currentChar;

        public (uint, uint) CurrentPosition { get => (_line, _colon - 1); }
        public char CurrentChar { get => currentChar; set => currentChar = value; }

        public Lexer(string text)
        {
            _text = text;
            CurrentChar = _text[(int)_pos];
        }

        public Token MakeToken(string type, string value)
        {
            return new Token(type, value, CurrentPosition);
        }

        public void Error()
        {
            throw new ArgumentException(string.Format("{0} Invalid character: {1}", CurrentPosition, CurrentChar));
        }

        public void Advance()
        // Advance the `pos` pointer and set the `CurrentChar` variable.
        {
            _pos += 1;
            if (_pos > _text.Length - 1)
            {
                //TODO check if the '\0' in a text could be a problem
                CurrentChar = Const.None;
            }
            else
            {
                if (CurrentChar == '\n')
                {
                    _line += 1;
                    _colon = 1;
                }
                else
                    _colon += 1;
                CurrentChar = _text[(int)_pos];
            }
        }

        public char Peek()
        {
            var peekPos = _pos + 1;
            if (peekPos > _text.Length - 1)
                return Char.MinValue;
            else
                return _text[(int)_pos];
        }

        public void SkipWhitespace()
        {
            while (Char.IsWhiteSpace(CurrentChar) && CurrentChar != Const.None)
                Advance();
        }

        public void SkipComment()
        {
            while (CurrentChar != '\n' && CurrentChar != Const.None)
                Advance();
        }

        public Token StringConst()
        {
            StringBuilder stringData = new StringBuilder();
            char previousChar;
            Advance();
            while (CurrentChar != '"' || Peek() == '"')
            {
                previousChar = CurrentChar;
                stringData.Append(CurrentChar);
                Advance();
                if (CurrentChar == '"' && previousChar == '"')
                    Advance();
            }
            Advance();

            return MakeToken(Const.StringConst, stringData.ToString());
        }

        public Token MakeCommentToken()
        {
            StringBuilder stringData = new StringBuilder();
            while (CurrentChar != '\n')
            {
                stringData.Append(CurrentChar);
                Advance();
            }
            Advance();

            return MakeToken(Const.Comment, stringData.ToString());
        }

        public Token Number()
        {
            StringBuilder stringData = new StringBuilder();
            Token token;

            while(Char.IsDigit(CurrentChar)  && CurrentChar != Const.None)
            {
                stringData.Append(CurrentChar);
                Advance();
            }
            if (CurrentChar == '.')
            {
                stringData.Append(CurrentChar);
                Advance();
                while (Char.IsDigit(CurrentChar) && CurrentChar != Const.None)
                {
                    stringData.Append(CurrentChar);
                    Advance();
                }
                token = MakeToken(Const.RealConst, stringData.ToString());
            }
            else
            {
                token = MakeToken(Const.IntegerConst, stringData.ToString());
            };

            return token;
        }

        private Token Id()
        {
            StringBuilder stringData = new StringBuilder();
            Token token;
            string id, type;

            while (Char.IsLetterOrDigit(CurrentChar) && CurrentChar != Const.None)
            {
                stringData.Append(CurrentChar);
                Advance();
            }
            id = stringData.ToString();

            if (Const.ReservedKeywords.Contains(id.ToUpper()))
                token = MakeToken(id.ToUpper(), id);
            else
                token = MakeToken(Const.Id, id);
            return token;
        }

        public Token GetNextToken()
        {
            //Lexical analyzer (also known as scanner or tokenizer)

            //This method is responsible for breaking a sentence
            //apart into tokens.One token at a time.

            while (CurrentChar != Const.None)
            {

                if (Char.IsWhiteSpace(CurrentChar))
                {
                    SkipWhitespace();
                    continue;
                }

                if (Char.IsLetter(CurrentChar))
                    return Id();

                if (Char.IsDigit(CurrentChar))
                    return Number();

                if (CurrentChar == '?')
                {
                    Advance();
                    return MakeToken(Const.Question, "?");
                }

                if (CurrentChar == '"')
                    return StringConst();

                if (CurrentChar == '=')
                {
                    Advance();
                    return MakeToken(Const.Equal, "=");
                }

                if (CurrentChar == ';')
                {
                    Advance();
                    return MakeToken(Const.Semi, ";");
                }

                if (CurrentChar == ':')
                {
                    Advance();
                    return MakeToken(Const.Colon, ":");
                }

                if (CurrentChar == ',')
                {
                    Advance();
                    return MakeToken(Const.Comma, ",");
                }

                if (CurrentChar == '+')
                {
                    Advance();
                    return MakeToken(Const.Plus, "+");
                }

                if (CurrentChar == '-')
                {
                    Advance();
                    return MakeToken(Const.Minus, "-");
                }

                if (CurrentChar == '*')
                {
                    Advance();
                    return MakeToken(Const.Mul, "*");
                }

                if (CurrentChar == '>')
                {
                    Advance();
                    if (CurrentChar == '=')
                    {
                        Advance();
                        return MakeToken(Const.GreaterEqual, ">=");
                    }
                    else
                        return MakeToken(Const.Greater, ">");
                }

                if (CurrentChar == '<')
                {
                    Advance();
                    if (CurrentChar == '=')
                    {
                        Advance();
                        return MakeToken(Const.LessEqual, "<=");
                    }
                    else if (CurrentChar == '>')
                    {
                        Advance();
                        return MakeToken(Const.Notequal, "<>");
                    }
                    else
                        return MakeToken(Const.Less, "<");
                }

                if (CurrentChar == '/' && Peek() == '/')
                {
                    Advance();
                    Advance();
                    SkipComment();
                    continue;
                }

                if (CurrentChar == '/')
                {
                    Advance();
                    return MakeToken(Const.FloatDiv, "/");
                }

                if (CurrentChar == '(')
                {
                    Advance();
                    return MakeToken(Const.Lparen, "(");
                }

                if (CurrentChar == ')')
                {
                    Advance();
                    return MakeToken(Const.Rparen, ")");
                }

                if (CurrentChar == '.')
                {
                    Advance();
                    return MakeToken(Const.Dot, ".");
                }

                Error();

            }
            
            return MakeToken(Const.EOF, Const.None);
        }
    }
}

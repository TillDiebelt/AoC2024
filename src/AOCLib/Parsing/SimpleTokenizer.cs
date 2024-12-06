using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace AOCLib.Parsing
{
    public enum ParseResult
    {
        IDLE = 0,
        BUSY = 1, //not valid
        FINISHED = 2, //valud token
        FINISHED_END = 3, //valid and no other char allowed
        FINISHED_ABORT = 4, //valid token but last accept ignored
        ABORT = -1 //not valid and wrong char received
    }

    public struct Token
    {
        public string token;
        public string type;
    }

    public abstract class ParseToken
    {
        public Token Token;
        public ParseResult State = ParseResult.IDLE;

        public ParseToken(string type) 
        {
            this.Token.type = type;
        }

        public abstract bool isStart(char c);

        public abstract ParseResult accept(char c);

        public abstract Token finalize();
    }

    public class BasicNumberToken : ParseToken
    {
        public long currentValue = 0;
        public BasicNumberToken(string type = "number") : base(type)
        {

        }

        public override ParseResult accept(char c)
        {
            if ((c >= 48 && c <= 57))
            {
                currentValue *= 10;
                currentValue += c.ToDigit();
                State = ParseResult.FINISHED;
                return ParseResult.FINISHED;
            }
            else
            {
                if (currentValue > 0)
                {
                    State = ParseResult.FINISHED_ABORT;
                    return ParseResult.FINISHED_ABORT;
                }

                return ParseResult.IDLE;
            }
        }

        public override bool isStart(char c)
        {
            return (c >= 49 && c <= 57);
        }

        public override Token finalize()
        {
            this.Token.token = this.currentValue.ToString();
            State = ParseResult.IDLE;
            this.currentValue = 0;
            return this.Token;
        }
    }


    public class OpenBracketToken : ParseToken
    {
        public string currentValue = "";
        public OpenBracketToken(string type = "(") : base(type)
        {

        }

        public override ParseResult accept(char c)
        {
            if (currentValue == "" && c == '(')
            {
                currentValue = "(";
                return ParseResult.FINISHED_END;
            }
            else
            {
                return ParseResult.ABORT;
            }
        }

        public override bool isStart(char c)
        {
            return (c == '(');
        }

        public override Token finalize()
        {
            this.Token.token = this.currentValue;
            this.currentValue = "";
            return this.Token;
        }
    }

    public class ClosingBracketToken : ParseToken
    {
        public string currentValue = "";
        public ClosingBracketToken(string type = ")") : base(type)
        {

        }

        public override ParseResult accept(char c)
        {
            if (currentValue == "" && c == ')')
            {
                currentValue = ")";
                return ParseResult.FINISHED_END;
            }
            else
            {
                return ParseResult.ABORT;
            }
        }

        public override bool isStart(char c)
        {
            return (c == ')');
        }

        public override Token finalize()
        {
            this.Token.token = this.currentValue;
            this.currentValue = "";
            return this.Token;
        }
    }
    public class CommaToken : ParseToken
    {
        public string currentValue = "";
        public CommaToken(string type = ",") : base(type)
        {

        }

        public override ParseResult accept(char c)
        {
            if (currentValue == "" && c == ',')
            {
                currentValue = ",";
                return ParseResult.FINISHED_END;
            }
            else
            {
                return ParseResult.ABORT;
            }
        }

        public override bool isStart(char c)
        {
            return (c == ',');
        }

        public override Token finalize()
        {
            this.Token.token = this.currentValue;
            this.currentValue = "";
            return this.Token;
        }
    }

    public class BasicLiteralToken : ParseToken
    {
        string tokenform = "";
        bool caseSensitive = false;
        public BasicLiteralToken(string tokenform, string type = "literal", bool casesensitive = false) :base(type)
        {
            this.tokenform = tokenform;
            this.caseSensitive = casesensitive;
        }

        string currentValue = "";
        public override ParseResult accept(char c)
        {
            if(tokenform.Substring(currentValue.Length).StartsWith(c) || !caseSensitive && tokenform.ToLower().Substring(currentValue.Length).StartsWith((c+"").ToLower()))
            {
                currentValue += c;
                if(currentValue == tokenform || !caseSensitive && currentValue.ToLower() == tokenform.ToLower())
                {
                    return ParseResult.FINISHED_END;
                }
                return ParseResult.BUSY;
            }
            else
            {
                return ParseResult.ABORT;
            }
        }

        public override bool isStart(char c)
        {
            return tokenform.StartsWith(c) || !caseSensitive && tokenform.ToLower().StartsWith((c + "").ToLower());
        }
        public override Token finalize()
        {
            this.Token.token = this.currentValue.ToString();
            this.currentValue = "";
            return this.Token;
        }
    }

    public class SimpleTokenizer
    {
        List<ParseToken> tokenParsers = new List<ParseToken>();
        public SimpleTokenizer(List<ParseToken> tokens)
        {
            tokenParsers.AddRange(tokens);
        }

        public List<Token> Tokenize(string text, bool addInvalids = false, bool shortestFirst = false)
        {
            List<Token> tokens = new List<Token>();
            for(int i = 0; i < text.Length; i++) 
            {
                if (tokenParsers.Any(x => x.isStart(text[i])))
                {
                    List<Token> results = new List<Token>();
                    foreach (var t in tokenParsers)
                    {
                        if (!t.isStart(text[i])) continue;
                        for(int j = i; j < text.Length; j++)
                        {
                            var res = t.accept(text[j]);
                            if (res == ParseResult.FINISHED && j == text.Length - 1)
                            {
                                results.Add(t.finalize());
                                break;
                            }
                            if (res == ParseResult.FINISHED_ABORT)
                            {
                                results.Add(t.finalize());
                                break;
                            }
                            if (res == ParseResult.FINISHED_END)
                            {
                                results.Add(t.finalize());
                                break;
                            }
                            if(res ==ParseResult.ABORT)
                            {
                                break;
                            }
                        }
                    }
                    foreach(var t in tokenParsers)
                    {
                        t.finalize();
                    }
                    if(results.Count > 0)
                    {
                        Token chosen;
                        if (shortestFirst)
                        {
                            chosen = (results.OrderBy(x => x.token.Length).First());
                        }
                        else
                        {
                            chosen = (results.OrderByDescending(x => x.token.Length).First());
                        }
                        tokens.Add(chosen);
                        i += chosen.token.Length-1;
                    }
                    else
                        if (addInvalids) tokens.Add(new Token { token = text[i].ToString(), type = "" });
                }
                else
                {
                    if(addInvalids) tokens.Add(new Token { token = text[i].ToString(), type = "" });
                }
            }
            return tokens;
        }
    }
}

using AOCLib.Parsing;

namespace Solutions
{
    public class SolverDay03 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            SimpleTokenizer tokenizer = new SimpleTokenizer(
                new List<ParseToken>() { 
                    new BasicLiteralToken("MUL", "mul"), 
                    new OpenBracketToken(), 
                    new BasicNumberToken(),
                    new ClosingBracketToken(), 
                    new CommaToken() }
                );
            var tokens = tokenizer.Tokenize(text, true);
            for(int i = 0; i < tokens.Count-5; i++)
            {
                if (tokens[i].type == "mul" && tokens[i+1].type == "(" && tokens[i+2].type == "number" && tokens[i+3].type == "," && tokens[i+4].type == "number" && tokens[i+5].type == ")")
                {
                    result += long.Parse(tokens[i + 2].token) * long.Parse(tokens[i + 4].token);
                }
            }
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {            
            //Solve
            long result = 0;
            bool enabled = true;
            SimpleTokenizer tokenizer = new SimpleTokenizer(
                new List<ParseToken>() {
                    new BasicLiteralToken("mul", "mul"),
                    new BasicLiteralToken("do", "do"),
                    new BasicLiteralToken("don't", "dont"),
                    new OpenBracketToken(),
                    new BasicNumberToken(),
                    new ClosingBracketToken(),
                    new CommaToken() }
                );
            var tokens = tokenizer.Tokenize(text, true);
            for (int i = 0; i < tokens.Count - 5; i++)
            {
                if (tokens[i].type == "mul" && tokens[i + 1].type == "(" && tokens[i + 2].type == "number" && tokens[i + 3].type == "," && tokens[i + 4].type == "number" && tokens[i + 5].type == ")")
                {
                    if(enabled) result += long.Parse(tokens[i + 2].token) * long.Parse(tokens[i + 4].token);
                }
                else if (tokens[i].type == "do" && tokens[i + 1].type == "(" && tokens[i + 2].type == ")")
                {
                    enabled = true;
                }
                else if (tokens[i].type == "dont" && tokens[i + 1].type == "(" && tokens[i + 2].type == ")")
                {
                    enabled = false;
                }
            }
            return result;
        }
    }
}

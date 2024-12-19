using AOCLib;

namespace Solutions
{
    public class SolverDay19 : ISolver
    {                
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            
            List<string> patterns = new();
            List<string> desings = new();

            patterns = lines[0].Split(",").Select(x => x.Trim()).ToList();
            for (int i = 2; i < lines.Length; i++)
            {
                desings.Add(lines[i]);
            }

            dyn.Clear();
            foreach(var d in desings)
            {
                if (DFS(ref patterns, d) > 0)
                    result++;
            }
            
            return result;
        }

        private static Dictionary<string, long> dyn = new();
        private long DFS(ref List<string> patterns, string design)
        {
            if(design.Length == 0)
            {
                return 1;
            }
            if (dyn.ContainsKey(design))
                return dyn[design];
            long valids = 0;
            foreach(var p in patterns)
            {
                if(design.StartsWith(p))
                {
                    valids += DFS(ref patterns, design.Substring(p.Length));
                }
            }
            dyn.TryAdd(design, valids);                
            return valids;
        }
        
        
        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            List<string> patterns = new();
            List<string> desings = new();

            patterns = lines[0].Split(",").Select(x => x.Trim()).ToList();
            for (int i = 2; i < lines.Length; i++)
            {
                desings.Add(lines[i]);
            }

            dyn.Clear();
            foreach (var d in desings)
            {
                result+= DFS(ref patterns, d);
            }

            return result;
        }
    }
}
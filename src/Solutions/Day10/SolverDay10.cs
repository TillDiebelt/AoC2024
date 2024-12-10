using AOCLib;

namespace Solutions
{
    public class SolverDay10 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;

            int[,] map = new int[lines.Length, lines[0].Length];

            List<(int x, int y)> starts = new List<(int x, int y)>();
            List<(int x, int y)> ends = new List<(int x, int y)>();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    {
                        map[i, j] = int.Parse(lines[i][j].ToString());
                        if (map[i, j] == 0)
                            starts.Add((j, i));
                        else if (map[i, j] == 9)
                            ends.Add((j, i));
                    }
                }
            }

            foreach(var start in starts)
            {
                result += Paths(map, start);
            }

            return result;
        }

        private long Paths(int[,] map, (int x, int y) start, bool uniqueFirstStep = true)
        {
            List<(int x, int y)> current = new List<(int x, int y)>(); //P1 would be better with HashSet
            current.Add(start);
            for (int i = 1; i <= 9; i++)
            {
                List<(int x, int y)> todo = new List<(int x, int y)>();
                foreach (var c in current)
                {
                    var neighbours = map.Neighbours(c.x, c.y);
                    neighbours = neighbours.Where(n => map[n.y, n.x] == i).ToList();
                    foreach (var n in neighbours)
                    {
                        if(uniqueFirstStep && todo.Contains(n))
                        {
                            continue;
                        }
                        todo.Add(n);
                    }
                }
                current = todo;
            }
            return current.Count;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            int[,] map = new int[lines.Length, lines[0].Length];

            List<(int x, int y)> starts = new List<(int x, int y)>();
            List<(int x, int y)> ends = new List<(int x, int y)>();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    {
                        map[i, j] = int.Parse(lines[i][j].ToString());
                        if (map[i, j] == 0)
                            starts.Add((j, i));
                        else if (map[i, j] == 9)
                            ends.Add((j, i));
                    }
                }
            }

            foreach (var start in starts)
            {
                result += Paths(map, start, false);
            }

            return result;
        }
    }
}

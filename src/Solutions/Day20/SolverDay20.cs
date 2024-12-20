using AOCLib;
using Microsoft.Diagnostics.Tracing.Parsers.JScript;
using System.Transactions;

namespace Solutions
{
    public class SolverDay20 : ISolver
    {                
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;

            //part 2 solution works here and is faster, but I like to keep it
            
            int[,] map = new int[lines.Length, lines[0].Length];
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '.')
                    {
                        map[i, j] = 0;
                    }
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    if (lines[i][j] == 'S')
                    {
                        map[i, j] = 0;
                        start = (j, i);
                    }
                    if (lines[i][j] == 'E')
                    {
                        map[i, j] = 0;
                        end = (j, i);
                    }
                }
            }

            var path = Pathfinder<int[,], (int x, int y)>.FindPath(
            map,
            (a) => new List<(int, int)>() { (start.x, start.y) },
                (a, b) => (b.x == end.x && b.y == end.y),
                (a, b) => Math.Abs(b.x - end.x) + Math.Abs(b.y - end.x),
                (a, b, c) => 1,
                (a, current) => {
                    var res = map.Neighbours(current.x, current.y);
                    res = res.Where(x => a[x.y, x.x] == 0).ToList();
                    return res;
                }
                );

            long normal = path.Count();

            for (int y = 1; y < map.GetUpperBound(0); y++)
            {
                for (int x = 1; x < map.GetUpperBound(1); x++)
                {
                    if (map[y,x] == 1)
                    {
                        map[y, x] = 0;

                        var path2 = Pathfinder<int[,], (int x, int y)>.FindPath(
                            map,
                            (a) => new List<(int, int)>() { (start.x, start.y) },
                                (a, b) => (b.x == end.x && b.y == end.y),
                                (a, b) => Math.Abs(b.x - end.x) + Math.Abs(b.y - end.x),
                                (a, b, c) => 1,
                                (a, current) => {
                                    var res = map.Neighbours(current.x, current.y);
                                    res = res.Where(x => a[x.y, x.x] == 0).ToList();
                                    return res;
                                }
                                );
                        if ((path2.Count() - normal <= -100) && path2.Contains((x,y)))
                        {
                            result++;
                        }

                        map[y, x] = 1;
                    }
                }
            }

            return result;
        }
              
        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            int[,] map = new int[lines.Length, lines[0].Length];
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();

                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '.')
                    {
                        map[i, j] = 0;
                    }
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    if (lines[i][j] == 'S')
                    {
                        map[i, j] = 0;
                        start = (j, i);
                    }
                    if (lines[i][j] == 'E')
                    {
                        map[i, j] = 0;
                        end = (j, i);
                    }
                }
            }

            //search for a "tunnel" which connects path from start and path from end with a shorter total
            /*  *
               ***
              **C**
               ***
                *
              
                check which of the * from cheatstart C are valid points and calculate cost with reverse path (if 2 cheats available)
             */
            Dictionary<(int x, int y), int> coordinateCosts = new();
            Dictionary<(int x, int y), int> coordinateCostsReverse = new();

            Queue<(int x, int y)> queue = new();
            queue.Enqueue(start);
            coordinateCosts.Add(start, 0);
            while(queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentCost = coordinateCosts[current];
                var neighs = map.Neighbours(current.x, current.y);
                foreach(var n in neighs)
                {
                    if (map[n.y, n.x] == 1 || coordinateCosts.ContainsKey(n))
                        continue;
                    coordinateCosts.Add(n, currentCost + 1);
                    queue.Enqueue(n);
                }
            }


            queue.Enqueue(end);
            coordinateCostsReverse.Add(end, 0);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentCost = coordinateCostsReverse[current];
                var neighs = map.Neighbours(current.x, current.y);
                foreach (var n in neighs)
                {
                    if (map[n.y, n.x] == 1 || coordinateCostsReverse.ContainsKey(n))
                        continue;
                    coordinateCostsReverse.Add(n, currentCost + 1);
                    queue.Enqueue(n);
                }
            }

            for(int y = 1; y < map.GetUpperBound(0); y++)
            {
                for (int x = 1; x < map.GetUpperBound(1); x++)
                {
                    if (map[y, x] == 1) continue;
                    (int x, int y) cheatStart = (x, y);

                    HashSet<(int x, int y)> seen = new();
                    seen.Add(cheatStart);
                    Queue<((int x, int y) pos, int cheats)> cheatQueue = new();
                    cheatQueue.Enqueue((cheatStart, 20));

                    while(cheatQueue.Count > 0)
                    {
                        var current = cheatQueue.Dequeue();
                        if (current.cheats == 0) continue;

                        var neighs = map.Neighbours(current.pos.x, current.pos.y);
                        foreach(var n in neighs)
                        {
                            if (seen.Contains(n)) continue;
                            seen.Add(n);
                            cheatQueue.Enqueue((n, current.cheats - 1));

                            if (map[n.y, n.x] == 0 
                                && 
                                (coordinateCostsReverse[n] + coordinateCosts[cheatStart] + (20-current.cheats) + 1 - coordinateCosts[end]) <= -100)
                            {
                                result++;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}

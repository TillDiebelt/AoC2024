using TillSharp.Math.Array;
using AOCLib;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Filters;
using System.Collections.Generic;
using System.Net.Http.Headers;
using BenchmarkDotNet.Disassemblers;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using System.Diagnostics;

namespace Solutions
{
    public class SolverDay16 : ISolver
    {        
        enum Rotation
        {
            UP = 0,
            RIGHT = 1,
            DOWN = 2,
            LEFT = 3
        }
        
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);
            Rotation rot = Rotation.RIGHT;

            int[,] map = new int[lines.Length, lines[0].Length];            
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    if (lines[i][j] == '.')
                    {
                        map[i, j] = 0;
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

            var path = Pathfinder<int[,], (int x, int y, Rotation rot)>.FindPath(
                map,
                (a) => new List<(int, int, Rotation rot)>() { (start.x, start.y, Rotation.RIGHT) },
                (a, b) => (b.x == end.x && b.y == end.y),
                (a, b) => Math.Abs(b.x - end.x) + Math.Abs(b.y - end.y),
                (a, b, c) => 1 + Math.Min((((b.rot - c.rot) % 4 + 4) % 4), 4 - (((b.rot - c.rot) % 4 + 4) % 4)) * 1000,
                (a, current) => {
                    var res = new List<(int x, int y, Rotation rot)>
                    {
                        (current.x - 1, current.y, Rotation.LEFT),
                        (current.x + 1, current.y, Rotation.RIGHT),
                        (current.x, current.y - 1, Rotation.UP),
                        (current.x, current.y + 1, Rotation.DOWN)
                    };
                    res = res.Where(x => a[x.y, x.x] == 0).ToList();                    
                    return res;
                }
                );

            foreach(var p in path)
            {
                result += 1 + Math.Min((((p.rot - rot) % 4 + 4) % 4), 4 - (((p.rot - rot) % 4 + 4) % 4)) * 1000;
                rot = p.rot;
            }
            
            return result;
        }

        List<List<(int x, int y)>> allPaths2 = new();
        int minCost = 999999999;
        private long DFS2(
            ref List<((int x, int y) start, (int x, int y) End, Rotation rotStart, Rotation rotEnd, int cost, int lenght, List<(int x, int y)> path)> paths, 
            (int x, int y) pos, 
            Rotation rot, 
            int cost, 
            (int x, int y) end, 
            List<(int x, int y)> seen
        )
        {
            seen.Add((pos.x, pos.y));
            if (pos.x == end.x && pos.y == end.y)
            {
                if (cost == minCost)
                {
                    allPaths2.Add(seen);
                }
                if (cost < minCost)
                {
                    allPaths2.Clear();
                    allPaths2.Add(seen);
                    minCost = cost;
                }
                return cost + 1;
            }
            var nexts = paths.Where(p => p.start == pos);
            foreach (var n in nexts)
            {
                if (seen.Contains((n.End.x, n.End.y))) continue;
                int newCost = cost + n.cost;
                if(n.rotStart != rot)
                {
                    newCost += 1000;
                }
                List<(int x, int y)> newSeen = new();
                newSeen.AddRange(seen);
                DFS2(ref paths, n.End, n.rotEnd, newCost, end, newSeen);
            }
            return -1;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);
            Rotation rot = Rotation.RIGHT;

            int[,] map = new int[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    if (lines[i][j] == '.')
                    {
                        map[i, j] = 0;
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

            var allpaths = Pathfinder<int[,], (int x, int y, Rotation rot)>.FindAllPaths(
                map,
                (a) => new List<(int, int, Rotation rot)>() { (start.x, start.y, Rotation.RIGHT) },
                (a, b) => (b.x == end.x && b.y == end.y),
                (a, b) => Math.Abs(b.x - end.x) + Math.Abs(b.y - end.y),
                (a, b, c) => 1 + Math.Min((((b.rot - c.rot) % 4 + 4) % 4), 4 - (((b.rot - c.rot) % 4 + 4) % 4)) * 1000,
                (a, current) => {
                    var res = new List<(int x, int y, Rotation rot)>
                    {
                        (current.x - 1, current.y, Rotation.LEFT),
                        (current.x + 1, current.y, Rotation.RIGHT),
                        (current.x, current.y - 1, Rotation.UP),
                        (current.x, current.y + 1, Rotation.DOWN)
                    };
                    res = res.Where(x => a[x.y, x.x] == 0).ToList();
                    return res;
                }
                );

            
            Queue<((int x, int y) current, (int x, int y) prev, (int x, int y) start, Rotation rotStart, Rotation rotCurrent, int cost, int lenght, List<(int, int)> path)> queue = new();
            List<(int x, int y, Rotation r)> seen = new();
            List<((int x, int y) start, (int x, int y) End, Rotation rotStart, Rotation rotEnd, int cost, int lenght, List<(int x, int y)> path)> paths = new();

            seen.Add((start.x, start.y, rot));
            queue.Enqueue(((start.x, start.y), (-1,-1), (start.x, start.y), rot, rot, 0, 0, new List<(int, int)>()));
            
            this.Walk(ref map, ref queue, ref seen, ref paths,end);
            paths = paths.OrderBy(x => x.start.y).ToList();
            this.DFS2(ref paths, start, rot, 0, end, new List<(int x, int y)>());

            HashSet<(int x, int y)> seenCross = new();
            seenCross.Add(start);
            HashSet<(int x, int y, int ex, int ey)> seenPoints = new();
            int write = 1;
            foreach (var l in allPaths2)
            {
                write++;
                (int x, int y) last = l.First();
                foreach(var p in l.Skip(1))
                {
                    seenCross.Add((p.x, p.y));
                    seenPoints.Add((last.x, last.y, p.x, p.y));
                    last = (p.x, p.y);
                    map[p.y, p.x] = write;
                }
            }
            

            HashSet<(int x, int y)> seenWalk = new();
            seenWalk.Add(start);
            //wrong solution, dont know why
            //foreach (var p in seenPoints)
            //{
            //    result += paths.Where(s => s.start.x == p.x && s.start.y == p.y && s.End.x == p.ex && s.End.y == p.ey).First().lenght;
            //}
            //result += seenWalk.Count();

            map[start.y, start.x] = write;
            foreach (var p in seenPoints)
            {
                var foo = paths.Where(s => s.start.x == p.x && s.start.y == p.y && s.End.x == p.ex && s.End.y == p.ey).OrderBy(x => x.cost).First().path;
                foreach(var f in foo)
                {
                    //seenWalk.Add(f);
                    if (map[f.y, f.x] == 0)
                    {
                        map[f.y, f.x] = 9;
                    }
                }
            }            
            
            foreach(var path in allpaths)
            {
                foreach(var p in path)
                {
                    map[p.y, p.x] = 8;
                    seenWalk.Add((p.x, p.y));
                }
            }
            result = seenWalk.Count();
            
            Console.WriteLine(minCost);
            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= map.GetUpperBound(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        Console.Write(" ");
                    }
                    else if (map[i, j] == 1)
                    {
                        Console.Write("#");
                    }
                    else if (map[i, j] == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(map[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (map[i, j] == 3)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write(map[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (map[i, j] == 4)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(map[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (map[i, j] == 8)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write(map[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(map[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            return result; 
        }

        private void Walk(ref int[,] map,
            ref Queue<((int x, int y) current, (int x, int y) prev, (int x, int y) start, Rotation rotStart, Rotation rotCurrent, int cost, int lenght, List<(int x, int y)> path)> queue, 
            ref List<(int x, int y, Rotation r)> seen, 
            ref List<((int x, int y) start, (int x, int y) End, Rotation rotStart, Rotation rotEnd, int cost, int lenght, List<(int x, int y)> path)> paths,
            (int x, int y) end)
        {
            while(queue.Count > 0)
            {
                var current = queue.Dequeue();
                var neighs = current.current.Neighbours();
                List<(int x, int y)> trueNei = new();
                foreach(var n in neighs)
                {
                    if (map[n.y, n.x] == 0)
                        trueNei.Add(n);
                }
                if(current.current == end)
                {
                    if (!paths.Contains((current.start, current.current, current.rotStart, current.rotCurrent, current.cost, current.lenght, current.path)))
                        paths.Add((current.start, current.current, current.rotStart, current.rotCurrent, current.cost, current.lenght, current.path));
                    continue;
                }
                if(trueNei.Count() > 2)
                {
                    if(!paths.Contains((current.start, current.current, current.rotStart, current.rotCurrent, current.cost, current.lenght, current.path)))
                        paths.Add((current.start, current.current, current.rotStart, current.rotCurrent, current.cost, current.lenght, current.path));
                    foreach (var n in trueNei)
                    {
                        Rotation r = current.rotCurrent;
                        if (n.x < current.current.x)
                        {
                            r = Rotation.RIGHT;//LEFT
                        }
                        if (n.x > current.current.x)
                        {
                            r = Rotation.RIGHT;
                        }
                        if (n.y > current.current.y)
                        {
                            r = Rotation.UP;//DOWN
                        }
                        if (n.y < current.current.y)
                        {
                            r = Rotation.UP;
                        }
                        int newCost = current.cost + 1;
                        if (r != current.rotCurrent)
                            newCost += 1000;
                        if (!seen.Contains((n.x, n.y, r)))
                        {                            
                            queue.Enqueue((n, current.current, current.current, r, r, 1, 0, new List<(int x, int y)>() { n }));
                            seen.Add((n.x, n.y, r));
                        }
                    }
                }
                else
                {
                    foreach (var n in trueNei)
                    {
                        if (n == current.prev) continue;
                        Rotation r = current.rotCurrent;
                        if (n.x < current.current.x)
                        {
                            r = Rotation.RIGHT;//LEFT
                        }
                        if (n.x > current.current.x)
                        {
                            r = Rotation.RIGHT;
                        }
                        if (n.y > current.current.y)
                        {
                            r = Rotation.UP;//DOWN
                        }
                        if (n.y < current.current.y)
                        {
                            r = Rotation.UP;
                        }
                        int newCost = current.cost + 1;
                        if (r != current.rotCurrent)
                            newCost += 1000;
                        if (!seen.Contains((n.x, n.y, r)))
                        {
                            List<(int x, int y)> newPath = new List<(int x, int y)>();
                            newPath.AddRange(current.path);
                            newPath.Add(n);
                            queue.Enqueue((n, current.current, current.start, current.rotStart, r, newCost, current.lenght + 1, newPath));
                            seen.Add((n.x, n.y, r));
                        }
                    }
                }

            }
        }
    }
}
using TillSharp.Math.Array;
using AOCLib;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Filters;
using System.Collections.Generic;

namespace Solutions
{
    public class SolverDay15 : ISolver
    {        
        enum Direction
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

            int[,] map = new int[lines.Length, lines[0].Length];
            List<Direction> moves = new();
            bool parse2 = false;
            (int x, int y) robot = (0,0);
            
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length  == 0)
                {
                    parse2 = true; 
                    continue;
                }
                if (parse2)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '<')
                        {
                            moves.Add(Direction.LEFT);
                        }
                        if (lines[i][j] == '^')
                        {
                            moves.Add(Direction.UP);
                        }
                        if (lines[i][j] == '>')
                        {
                            moves.Add(Direction.RIGHT);
                        }
                        if (lines[i][j] == 'v')
                        {
                            moves.Add(Direction.DOWN);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '#')
                        {
                            map[i, j] = 2;
                        }
                        if (lines[i][j] == '.')
                        {
                            map[i, j] = 0;
                        }
                        if (lines[i][j] == 'O')
                        {
                            map[i, j] = 1;
                        }
                        if (lines[i][j] == '@')
                        {
                            map[i, j] = 0;
                            robot = (j, i);
                        }
                    }
                }
            }

            foreach(var move in moves)
            {
                bool movable = false;
                switch (move)
                {
                    case Direction.UP:
                        for(int dy = robot.y-1; dy > 0; dy--)
                        {
                            if (map[dy, robot.x] == 2)
                            {
                                break;
                            }
                            if (map[dy, robot.x] == 0)
                            {
                                movable = true;
                                for (int my = dy+1; my <= robot.y; my++)
                                {
                                    map[my - 1, robot.x] = map[my, robot.x];
                                }
                                robot = (robot.x, robot.y - 1);
                                map[robot.y, robot.x] = 0;
                                break;
                            }
                        }
                        break;
                    case Direction.RIGHT:
                        for (int dx = robot.x + 1; dx < map.GetUpperBound(1); dx++)
                        {
                            if (map[robot.y, dx] == 2)
                            {
                                break;
                            }
                            if (map[robot.y, dx] == 0)
                            {
                                movable = true;
                                for (int mx = dx - 1; mx >= robot.x; mx--)
                                {
                                    map[robot.y, mx + 1] = map[robot.y, mx];
                                }
                                map[robot.y, robot.x] = 0;
                                robot = (robot.x +1, robot.y);
                                break;
                            }
                        }
                        break;
                    case Direction.DOWN:
                        for (int dy = robot.y + 1; dy < map.GetUpperBound(0); dy++)
                        {
                            if (map[dy, robot.x] == 2)
                            {
                                break;
                            }
                            if (map[dy, robot.x] == 0)
                            {
                                movable = true;
                                for (int my = dy - 1; my >= robot.y; my--)
                                {
                                    map[my + 1, robot.x] = map[my, robot.x];
                                }
                                map[robot.y, robot.x] = 0;
                                robot = (robot.x, robot.y + 1);
                                break;
                            }
                        }
                        break;
                    case Direction.LEFT:
                        for (int dx = robot.x - 1; dx > 0; dx--)
                        {
                            if (map[robot.y, dx] == 2)
                            {
                                break;
                            }
                            if (map[robot.y, dx] == 0)
                            {
                                movable = true;
                                for (int mx = dx + 1; mx <= robot.x; mx++)
                                {
                                    map[robot.y, mx - 1] = map[robot.y, mx];
                                }
                                map[robot.y, robot.x] = 0;
                                robot = (robot.x - 1, robot.y);
                                break;
                            }
                        }
                        break;
                }
            }

            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= map.GetUpperBound(1); j++)
                {
                    if (map[i,j] == 1)
                    {
                        result += 100 * i + j;
                    }
                }
            }

            //map.Print();

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            int split = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    split = i;
                    break;
                }
            }
            int[,] map = new int[split, lines[0].Length*2];
            List<Direction> moves = new();
            List<(int lx, int rx, int y)> boxs = new();
            List<(int x, int y)> walls = new();
            bool parse2 = false;
            (int x, int y) robot = (0, 0);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    parse2 = true;
                    continue;
                }
                if (parse2)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '<')
                        {
                            moves.Add(Direction.LEFT);
                        }
                        if (lines[i][j] == '^')
                        {
                            moves.Add(Direction.UP);
                        }
                        if (lines[i][j] == '>')
                        {
                            moves.Add(Direction.RIGHT);
                        }
                        if (lines[i][j] == 'v')
                        {
                            moves.Add(Direction.DOWN);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '#')
                        {
                            map[i, j * 2] = 2;
                            map[i, j * 2 + 1] = 2;
                            walls.Add((j * 2, i));
                            walls.Add((j * 2 + 1, i));
                        }
                        if (lines[i][j] == '.')
                        {
                            map[i, j * 2] = 0;
                            map[i, j * 2 + 1] = 0;
                        }
                        if (lines[i][j] == 'O')
                        {
                            //map only for printing so we no longer move the boxes on the map 
                            //map[i, j * 2] = 1;
                            //map[i, j * 2 + 1] = 1;
                            boxs.Add((j * 2, j * 2 + 1, i));
                        }
                        if (lines[i][j] == '@')
                        {
                            map[i, j * 2] = 0;
                            map[i, j * 2 + 1] = 0;
                            robot = (j*2, i);
                        }
                    }
                }
            }

            foreach (var move in moves)
            {
                bool movable = false;
                int dx = 0;
                int dy = 0;
                switch (move)
                {
                    case Direction.UP:
                        if (walls.Any(x => x.x == robot.x && x.y == robot.y - 1)) break;
                        dx = 0;
                        dy = -1;
                        break;
                    case Direction.RIGHT:
                        dx = 1;
                        dy = 0;
                        break;
                    case Direction.DOWN:
                        dx = 0;
                        dy = 1;
                        break;
                    case Direction.LEFT:
                        dx = -1;
                        dy = 0;
                        break;
                }

                var connected = Connected(boxs, walls, dx, dy, robot.x, robot.y);
                if(connected.Item1)
                {
                    foreach (var c in connected.Item2)
                    {
                        boxs.Remove(c);
                    }
                    foreach (var c in connected.Item2)
                    {
                        boxs.Add((c.lx + dx, c.rx + dx, c.y + dy));
                    }
                    if (map[robot.y + dy, robot.x + dx] == 0)
                    {
                        robot = (robot.x + dx, robot.y + dy);
                    }
                }               
            }

            //foreach (var b in boxs)
            //{
            //    map[b.y, b.lx] = 1;
            //    map[b.y, b.rx] = 1;
            //}
            //map.Print();

            foreach (var b in boxs)
            {
                result += b.y * 100 + b.lx;
            }

            return result;
        }

        private (bool, List<(int lx, int rx, int y)>) Connected(List<(int lx, int rx, int y)> boxes, List<(int x, int y)> walls, int dx, int dy, int sx, int sy)
        {
            List<(int lx, int rx, int y)> result = new();
            var startBox = boxes.Where(b => (b.lx == (sx + dx) || b.rx == sx + dx) && (b.y == sy + dy)).FirstOrDefault();
            if (startBox == default) return (true, result);
            result.Add(startBox);
            bool added = true;
            while(added)
            {
                added = false;
                List<(int lx, int rx, int y)> todo = new();
                foreach (var b in result)
                {
                    var neighs = boxes.Where(x => (x.lx == b.lx + dx || x.rx == b.lx + dx || x.lx == b.rx + dx) && x.y == b.y + dy).ToList();
                    foreach (var n in neighs)
                    {
                        if (!result.Contains(n) && !todo.Contains(n))
                        {
                            todo.Add(n);
                            added = true;
                        }
                    }
                }
                result.AddRange(todo);
            }
            foreach(var r in result)
            {
                if (walls.Any(x => x.x == r.lx + dx && x.y == r.y + dy) || walls.Any(x => x.x == r.rx + dx && x.y == r.y + dy))
                {
                    result.Clear();
                    return (false, result);
                }
            }
            return (true, result);
        }
    }
}
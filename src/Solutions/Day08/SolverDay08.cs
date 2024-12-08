using AOCLib;
using System.ComponentModel.DataAnnotations;
using TillSharp.Math.Array;

namespace Solutions
{
    public class SolverDay08 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;

            char[,] map = new char[lines.Length, lines[0].Length];
            HashSet<(int,int)> antinodes = new();
            Dictionary<char, List<(int x, int y)>> antennas = new();
            for (int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j];
                    if (map[i, j] != '.')
                    {
                        if (antennas.ContainsKey(map[i, j]))
                        {
                            antennas[map[i, j]].Add((j, i));
                        }
                        else
                        {
                            antennas.Add(map[i, j], new List<(int, int)>() { (j, i) });
                        }
                    }
                }
            }

            foreach(var antannaGroup in antennas)
            {
                foreach(var antananasA in antannaGroup.Value)
                {
                    foreach(var antananasB in antannaGroup.Value)
                    {
                        int distX = antananasA.x - antananasB.x;
                        int distY = antananasA.y - antananasB.y;

                        int newX = antananasA.x + distX;
                        int newY = antananasA.y + distY;
                        if (distX == 0 && distY == 0) continue;
                        if(map.InBounds(newX, newY))
                        {
                            antinodes.Add((newX, newY));
                            if (map[antananasA.y + distY, antananasA.x + distX] == '.')
                            {
                                map[antananasA.y + distY, antananasA.x + distX] = '#'; //debugging
                            }
                        }
                    }
                }
            }

            result = antinodes.Count();

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            char[,] map = new char[lines.Length, lines[0].Length];
            HashSet<(int, int)> antinodes = new();
            Dictionary<char, List<(int x, int y)>> antennas = new();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j];
                    if (map[i, j] != '.')
                    {
                        if (antennas.ContainsKey(map[i, j]))
                        {
                            antennas[map[i, j]].Add((j, i));
                        }
                        else
                        {
                            antennas.Add(map[i, j], new List<(int, int)>() { (j, i) });
                        }
                    }
                }
            }

            foreach (var antannaGroup in antennas)
            {
                foreach (var antananasA in antannaGroup.Value)
                {
                    foreach (var antananasB in antannaGroup.Value)
                    {
                        int distX = antananasA.x - antananasB.x;
                        int distY = antananasA.y - antananasB.y;

                        if (distX == 0 && distY == 0) continue;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            int moveX = antananasA.x + i * distX;
                            int moveY = antananasA.y + i * distY;
                            if (map.InBounds(moveX, moveY))
                            {
                                antinodes.Add((moveX, moveY));
                                if (map[moveY, moveX] == '.')
                                {
                                    map[moveY, moveX] = '#'; //debugging
                                }
                            }
                            else
                                break;
                        }
                    }
                }
            }

            result = antinodes.Count();

            return result;
        }
    }
}

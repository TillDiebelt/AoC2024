using AOCLib;
using System.Drawing;

namespace Solutions
{
    public class SolverDay12 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;

            int[,] map = new int[lines.Length, lines[0].Length];
            int[,] seen = new int[lines.Length, lines[0].Length]; //filled floodfill areas

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j];
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var ff = Floodfill(ref map, ref seen, (j, i));
                    result += ff.perimeter * ff.area;
                }
            }

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            int[,] map = new int[lines.Length, lines[0].Length];
            int[,] seen = new int[lines.Length, lines[0].Length]; //filled floodfill areas

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j];
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var ff = Floodfill(ref map, ref seen, (j, i));
                    result += ff.area * ff.edges;
                }
            }

            return result;
        }





        private (int perimeter, int area, int edges) Floodfill(ref int[,] map, ref int[,] seen, (int x, int y) start)
        {
            List<(int x, int y)> perimeter = new(); //all fields which touch the area
            int area = 0;
            int edges = 0;
            if (seen[start.y, start.x] != 0)
                return (0, 0, 0);
            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
            queue.Enqueue(start);
            int startVal = map[start.y, start.x];
            int color = 1; //anything works because we are only interested in the count

            while (queue.Count > 0)
            {
                (int x, int y) current = queue.Dequeue();
                seen[current.y, current.x] = color;
                area++;

                edges += EdgeStarts(ref map, current);

                var neighs = map.Neighbours(current.x, current.y, true);
                foreach (var n in neighs)
                {
                    if (map.InBounds(n.x, n.y) && map[n.y, n.x] == startVal)
                    {
                        if (seen[n.y, n.x] == 0)
                        {
                            seen[n.y, n.x] = color;
                            if (!queue.Contains(n))
                            {
                                queue.Enqueue(n);
                            }
                        }
                    }
                    else
                    {
                        perimeter.Add(current);
                    }
                }
            }

            return (perimeter.Count(), area, edges);
        }

        //only count the first element of each edge (starting points, up->down, left->right)
        //there is possibly a cleaner way to do this with the perimeter list and merging neighbors to the same edge
        private int EdgeStarts(ref int[,] map, (int x, int y) current)
        {
            int edges = 0;
            int color = map[current.y, current.x];

            //is upper same color
            if (map.InBounds(current.x, current.y - 1) && map[current.y - 1, current.x] == color)
            {
                //is left same color
                if (map.InBounds(current.x - 1, current.y - 1) && map[current.y - 1, current.x - 1] == color)
                {
                    //is left same color
                    if (map.InBounds(current.x - 1, current.y) && map[current.y, current.x - 1] == color)
                    {

                    }
                    else
                    {
                        edges++;
                    }
                }

                //is right same color
                if (map.InBounds(current.x + 1, current.y - 1) && map[current.y - 1, current.x + 1] == color)
                {
                    //is right same color
                    if (map.InBounds(current.x + 1, current.y) && map[current.y, current.x + 1] == color)
                    {
                    }
                    else
                    {
                        edges++;
                    }
                }
            }
            else
            {
                //is left same color
                if (map.InBounds(current.x - 1, current.y) && map[current.y, current.x - 1] == color)
                {

                }
                else
                {
                    edges++;
                }
                //is right same color
                if (map.InBounds(current.x + 1, current.y) && map[current.y, current.x + 1] == color)
                {
                }
                else
                {
                    edges++;
                }
            }

            //is left same color
            if (map.InBounds(current.x - 1, current.y) && map[current.y, current.x - 1] == color)
            {
                //is upper same color
                if (map.InBounds(current.x - 1, current.y - 1) && map[current.y - 1, current.x - 1] == color)
                {
                    //is upper same color
                    if (map.InBounds(current.x, current.y - 1) && map[current.y - 1, current.x] == color)
                    {
                    }
                    else
                    {
                        edges++;
                    }
                }

                //is lower same color
                if (map.InBounds(current.x - 1, current.y + 1) && map[current.y + 1, current.x - 1] == color)
                {
                    //is lower same color
                    if (map.InBounds(current.x, current.y + 1) && map[current.y + 1, current.x] == color)
                    {
                    }
                    else
                    {
                        edges++;
                    }
                }
            }
            else
            {
                //is upper same color
                if (map.InBounds(current.x, current.y - 1) && map[current.y - 1, current.x] == color)
                {
                }
                else
                {
                    edges++;
                }
                //is lower same color
                if (map.InBounds(current.x, current.y + 1) && map[current.y + 1, current.x] == color)
                {
                }
                else
                {
                    edges++;
                }
            }
            return edges;
        }
    }
}

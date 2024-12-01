using System.Numerics;

namespace AOCLib
{
    public static class Extenders
    {
        /*
         implement more algorithms:
            uniform
            floyd
            floyd marschall
            ...
         */
        /*
        public static IEnumerable<(int x, int y)> FindPath<T>(this T[,] map, (int x, int y) start, (int x, int y) end, Func<T, T, bool> filter)
        {
            Pathfinder<T> pathfinder = new Pathfinder<T>(map);
            if (filter != null)
                pathfinder.Filter = filter;
            return pathfinder.FindPath(start, end);
        }*/

        public static int ToDigit(this char self)
        {
            return self - 48;
        }

        public static T GetValue<T>(this T[,] self, (int x, int y) pos)
        {
            return self[pos.y, pos.x];
        }

        public static IEnumerable<(int x, int y)> ToOffsets(this IEnumerable<(int, int)> self, (int x, int y) origin)
        {
            List<(int x, int y)> Offsets = new List<(int x, int y)>();
            foreach (var item in self)
            {
                Offsets.Add((item.Item1 - origin.x, item.Item2 - origin.y));
            }
            return Offsets;
        }

        public static (int x, int y) ToOffset(this (int, int) self, (int x, int y) origin)
        {
            return (self.Item1 - origin.x, self.Item2 - origin.y);
        }

        public static IEnumerable<(int x, int y)> Neighbours<T>(this T[][] self, int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0) neighbours.Add((x - 1, y));
            if (x + 1 < self[y].Length) neighbours.Add((x + 1, y));
            if (y - 1 >= 0) neighbours.Add((x, y - 1));
            if (y + 1 < self.Length) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static IEnumerable<(int x, int y)> Neighbours<T>(this T[,] self, int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0) neighbours.Add((x - 1, y));
            if (x + 1 <= self.GetUpperBound(1)) neighbours.Add((x + 1, y));
            if (y - 1 >= 0) neighbours.Add((x, y - 1));
            if (y + 1 <= self.GetUpperBound(0)) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static IEnumerable<(int x, int y)> NeighboursDiag<T>(this T[][] self, int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0)
            {
                neighbours.Add((x - 1, y));
                if (y - 1 >= 0) neighbours.Add((x - 1, y - 1));
                if (y + 1 < self.Length) neighbours.Add((x - 1, y + 1));
            }
            if (x + 1 < self[y].Length)
            {
                neighbours.Add((x + 1, y));
                if (y - 1 >= 0) neighbours.Add((x + 1, y - 1));
                if (y + 1 < self.Length) neighbours.Add((x + 1, y + 1));
            }
            if (y - 1 >= 0) neighbours.Add((x, y - 1));
            if (y + 1 < self[y].Length) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static IEnumerable<(int x, int y)> NeighboursDiag<T>(this T[,] self, int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0)
            {
                neighbours.Add((x - 1, y));
                if (y - 1 >= 0) neighbours.Add((x - 1, y - 1));
                if (y + 1 <= self.GetUpperBound(0)) neighbours.Add((x - 1, y + 1));
            }
            if (x + 1 <= self.GetUpperBound(1))
            {
                neighbours.Add((x + 1, y));
                if (y - 1 >= 0) neighbours.Add((x + 1, y - 1));
                if (y + 1 <= self.GetUpperBound(0)) neighbours.Add((x + 1, y + 1));
            }
            if (y - 1 >= 0) neighbours.Add((x, y - 1));
            if (y + 1 <= self.GetUpperBound(0)) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static T[,] MapApply<T>(this T[,] self, Func<T, T> func)
        {
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    self[y, x] = func(self[y, x]);
                }
            }
            return self;
        }

        public static IEnumerable<T> MapUntil<T>(this T[] self, Func<T, T> func, T stop)
        {
            for (int i = 0; i <= self.GetUpperBound(0); i++)
            {
                if (self[i].Equals(stop))
                {
                    return self.Take(i);
                }
                self[i] = func(self[i]);
            }
            return self;
        }

        public static int CountUntil<T>(this T[] self, T stop)
        {
            return self.MapUntil(x => x, stop).Count();
        }

        public static BigInteger Product(this IEnumerable<BigInteger> self)
        {
            return self.Aggregate(1, (BigInteger x, BigInteger y) => x * y);
        }

        public static BigInteger FastProduct(this IEnumerable<BigInteger> self)
        {
            if (self.Count() == 0)
            {
                return 1;
            }
            List<BigInteger> current = self.ToList();
            List<BigInteger> todo = new List<BigInteger>();
            while (current.Count > 1)
            {
                for (int i = 0; i < current.Count; i += 2)
                {
                    if (i + 1 < current.Count)
                    {
                        todo.Add(current[i] * current[i + 1]);
                    }
                    else
                    {
                        todo.Add(current[i]);
                    }
                }
                current = todo;
                todo = new List<BigInteger>();
            }
            return current[0];
        }

        public static BigInteger FastProduct<T>(this IEnumerable<T> self, Func<T, BigInteger> convert)
        {
            if (self.Count() == 0)
            {
                return 1;
            }
            List<BigInteger> current = self.Select(convert).ToList();
            List<BigInteger> todo = new List<BigInteger>();
            while (current.Count > 1)
            {
                for (int i = 0; i < current.Count; i += 2)
                {
                    if (i + 1 < current.Count)
                    {
                        todo.Add(current[i] * current[i + 1]);
                    }
                    else
                    {
                        todo.Add(current[i]);
                    }
                }
                current = todo;
                todo = new List<BigInteger>();
            }
            return current[0];
        }

        public static BigInteger Product<T>(this IEnumerable<T> self, Func<T, BigInteger> convert)
        {
            return self.Aggregate((BigInteger)1, (BigInteger x, T y) => convert(y) * x);
        }

        public static T[][] MapApply<T>(this T[][] self, Func<T, T> func)
        {
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x < self[y].Length; x++)
                {
                    self[y][x] = func(self[y][x]);
                }
            }
            return self;
        }

        public static R[,] Map<T, R>(this T[,] self, Func<T, R> func)
        {
            R[,] result = new R[self.GetUpperBound(0) + 1, self.GetUpperBound(1) + 1];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    result[y, x] = func(self[y, x]);
                }
            }
            return result;
        }

        public static R[][] Map<T, R>(this T[][] self, Func<T, R> func)
        {
            R[][] result = new R[self.GetUpperBound(0) + 1][];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                result[y] = new R[self[y].Length];
                for (int x = 0; x < self[y].Length; x++)
                {
                    result[y][x] = func(self[y][x]);
                }
            }
            return result;
        }
    }
}
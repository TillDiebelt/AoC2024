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

        public static bool InBounds<T>(this T[,] self, int x, int y)
        {
            return (x >= self.GetLowerBound(1) && x <= self.GetUpperBound(1) && y >= self.GetLowerBound(0) && y <= self.GetUpperBound(0));
        }
        
        public static bool InBounds<T>(this T[,] self, (int x, int y) coord)
        {
            return (coord.x >= self.GetLowerBound(1) && coord.x <= self.GetUpperBound(1) && coord.y >= self.GetLowerBound(0) && coord.y <= self.GetUpperBound(0));
        }

        public static List<long> GetNumbers(this string self, bool allowNegative = true)
        {
            List<long> numbers = new List<long>();
            long current = 0;
            bool foundNumber = false;
            bool neg = false;
            for(int i = 0; i < self.Length; i++)
            {
                if (self[i] >= 48 && self[i] <= 57)
                {
                    foundNumber = true;
                    current *= 10;
                    current += self[i] - 48;
                }
                else
                {
                    if(foundNumber)
                    {
                        if (neg && allowNegative)
                            current *= -1;
                        numbers.Add(current);
                    }
                    foundNumber = false;
                    neg = false;
                    if (self[i] == '-')
                    {
                        neg = true;
                    }
                    current = 0;
                }
            }
            if (neg)
                current *= -1;
            if (foundNumber)
                numbers.Add(current);

            return numbers;
        }

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

        public static IEnumerable<(int x, int y)> Neighbours<T>(this T[][] self, int x, int y, bool ignoreBounds = false)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0 || ignoreBounds) neighbours.Add((x - 1, y));
            if (x + 1 < self[y].Length || ignoreBounds) neighbours.Add((x + 1, y));
            if (y - 1 >= 0 || ignoreBounds) neighbours.Add((x, y - 1));
            if (y + 1 < self.Length || ignoreBounds) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static IEnumerable<(int x, int y)> Neighbours(this (int x, int y) self)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            neighbours.Add((self.x - 1, self.y));
            neighbours.Add((self.x + 1, self.y));
            neighbours.Add((self.x, self.y - 1));
            neighbours.Add((self.x, self.y + 1));
            return neighbours;
        }

        public static IEnumerable<(int x, int y)> Neighbours<T>(this T[,] self, int x, int y, bool ignoreBounds = false)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x - 1 >= 0 || ignoreBounds) neighbours.Add((x - 1, y));
            if (x + 1 <= self.GetUpperBound(1) || ignoreBounds) neighbours.Add((x + 1, y));
            if (y - 1 >= 0 || ignoreBounds) neighbours.Add((x, y - 1));
            if (y + 1 <= self.GetUpperBound(0) || ignoreBounds) neighbours.Add((x, y + 1));
            return neighbours;
        }

        public static T[,] RotateRight<T>(this T[,] self)
        {
            T[,] rotated = new T[self.GetUpperBound(1) + 1, self.GetUpperBound(0) + 1];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    rotated[x, self.GetUpperBound(0) - y] = self[y, x];
                }
            }
            return rotated;
        }

        public static T[,] RotateLeft<T>(this T[,] self)
        {
            T[,] rotated = new T[self.GetUpperBound(1) + 1, self.GetUpperBound(0) + 1];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    rotated[self.GetUpperBound(1) - x, y] = self[y, x];
                }
            }
            return rotated;
        }

        public static T[,] FlipVertical<T>(this T[,] self)
        {
            T[,] flipped = new T[self.GetUpperBound(0) + 1, self.GetUpperBound(1) + 1];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    flipped[y, self.GetUpperBound(1) - x] = self[y, x];
                }
            }
            return flipped;
        }

        public static T[,] FlipHorizontal<T>(this T[,] self)
        {
            T[,] flipped = new T[self.GetUpperBound(0) + 1, self.GetUpperBound(1) + 1];
            for (int y = 0; y <= self.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= self.GetUpperBound(1); x++)
                {
                    flipped[self.GetUpperBound(0) - y, x] = self[y, x];
                }
            }
            return flipped;
        }

        public static long mod(this long self, long modulo)
        {
            return (self % modulo + modulo) % modulo;
        }

        //if skipoutside is true, the function will not return positions that are outside the bounds of the map
        //dontCares can be inside the mask where it does not mapper if it is the same as the map
        //returns the top left position of the mask if it fits
        public static IEnumerable<(int x, int y)> FitsMask<T>(this T[,] self, T[,] mask, T dontCare = default(T))
        {
            for (int y = 0; y <= mask.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= mask.GetUpperBound(1); x++)
                {
                    bool fits = true;
                    for (int maskX = 0; maskX <= mask.GetUpperBound(1); maskX++)
                    {
                        if(!fits)
                            break;
                        for (int maskY = 0; maskY <= mask.GetUpperBound(0); maskY++)
                        {
                            if (!self.InBounds(x + maskX, y + maskY))
                            {
                                fits = false;
                                break;
                            }
                            if (mask[maskY, maskX].Equals(self[y + maskY, x + maskX]) || dontCare != null && !dontCare.Equals(default(T)) && dontCare.Equals(mask[maskY, maskX]))
                            {
                            }
                            else
                            {
                                fits = false;
                                break;
                            }
                        }
                    }
                    if (fits)
                    {
                        yield return (x, y);
                    }
                }
            }
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
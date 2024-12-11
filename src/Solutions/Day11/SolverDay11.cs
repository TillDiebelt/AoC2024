using AOCLib;

namespace Solutions
{
    public class SolverDay11 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;

            var nums = lines[0].GetNumbers();

            foreach (var n in nums)
            {
                result += GetStones(n, 25);
            }

            return result;
        }


        //this will be resused for part 2
        private static Dictionary<(long, int), long> blinks = new Dictionary<(long, int), long>();
        private static long GetStones(long n, int count)
        {
            if(blinks.ContainsKey((n, count)))
            {
                return blinks[(n, count)];
            }

            if (count == 0)
            {
                return 1;
            }
            long res = 0;
            if (n == 0)
            {
                res =(GetStones(1,count-1));
            }
            else if ((n + "").Length % 2 == 0)
            {
                long v1 = long.Parse((n + "").Substring(0, (n + "").Length / 2));
                long v2 = long.Parse((n + "").Substring(((n + "").Length / 2), (n + "").Length / 2));

                res = (GetStones(v1, count - 1)) + (GetStones(v2, count - 1));
            }
            else
            {
                 res = (GetStones(n * 2024, count - 1));
            }

            if (!blinks.ContainsKey((n, count)))
            {
                blinks.Add((n, count), res);
            }
            return res;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            var nums = lines[0].GetNumbers();

            foreach(var n in nums)
            {
                result += GetStones(n, 75);
            }

            return result;
        }
    }
}

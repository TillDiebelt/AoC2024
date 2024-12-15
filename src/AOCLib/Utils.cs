namespace AOCLib
{
    public static class Utils
    {
        public static long GaussSum(long end, long start = 0)
        {
            return (end * (end + 1)) / 2 - (start * (start + 1)) / 2;
        }
        
        /*
         not tested for diagonals
         */
        public static long Shoelace(this List<(int x, int y)> path)
        {
            long result = 0;
            long edges = 0;
            for (int i = 1; i < path.Count; i++)
            {
                result += path[i - 1].x * path[i].y - path[i].x * path[i - 1].y;
                edges += Math.Abs(path[i - 1].x - path[i].x) + Math.Abs(path[i - 1].y - path[i].y);
            }
            return Math.Abs(result) / 2 + edges / 2 + 1;
        }

        /*
         not tested for diagonals
         */
        public static long Shoelace(this List<(long x, long y)> path)
        {
            long result = 0;
            long edges = 0;
            for (int i = 1; i < path.Count; i++)
            {
                result += path[i - 1].x * path[i].y - path[i].x * path[i - 1].y;
                edges += Math.Abs(path[i - 1].x - path[i].x) + Math.Abs(path[i - 1].y - path[i].y);
            }
            return Math.Abs(result) / 2 + edges / 2 + 1;
        }
    }
}

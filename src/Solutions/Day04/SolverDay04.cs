namespace Solutions
{
    public class SolverDay04 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {
            //neigbour function as raycast with n (1..inf) elements (lazy?)
            //Solve
            long result = 0;
            char[][] chars = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                chars[i] = new char[lines[i].Length];
                for(int j = 0; j < lines[i].Length; j++)
                {
                    chars[i][j] = lines[i][j];
                }
            }
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (chars[i][j] == 'X')
                    {
                        result += XmasCount(chars, j, i);
                    }
                }
            }

            return result;
        }

        public static bool isX(char[][] chars, int x, int y)
        {
            if (chars[y-1][x-1] == 'M' && chars[y + 1][x + 1] == 'S' || chars[y - 1][x - 1] == 'S' && chars[y + 1][x + 1] == 'M')
            {
                if (chars[y - 1][x + 1] == 'M' && chars[y + 1][x - 1] == 'S' || chars[y - 1][x + 1] == 'S' && chars[y + 1][x - 1] == 'M')
                {
                    return true;
                }
            }
            return false;
        }

        public static int XmasCount(char[][] chars, int x, int y) 
        {
            string XMAS = "XMAS";
            int count = 0;
            //can go right
            if(x <= chars[y].Length - 4)
            {
                for(int ix = 0; ix <= 3; ix++)
                {
                    if (chars[y][x + ix] != XMAS[ix])
                    {
                        break;
                    }
                    if(ix == 3)
                    {
                        count++;
                    }
                }
                //can go down left
                if (y <= chars.Length - 4)
                { 
                    for (int ix = 0; ix <= 3; ix++)
                    {
                        if (chars[y+ix][x+ix] != XMAS[ix])
                        {
                            break;
                        }
                        if (ix == 3)
                        {
                            count++;
                        }
                    }
                }
                //can go up left
                if (y >= 3)
                {
                    for (int ix = 0; ix <= 3; ix++)
                    {
                        if (chars[y - ix][x + ix] != XMAS[ix])
                        {
                            break;
                        }
                        if (ix == 3)
                        {
                            count++;
                        }
                    }
                }
            }

            //can go left
            if (x >= 3)
            {
                for (int ix = 0; ix <= 3; ix++)
                {
                    if (chars[y][x - ix] != XMAS[ix])
                    {
                        break;
                    }
                    if (ix == 3)
                    {
                        count++;
                    }
                }
                //can go down left
                if (y <= chars.Length - 4)
                {
                    for (int ix = 0; ix <= 3; ix++)
                    {
                        if (chars[y + ix][x - ix] != XMAS[ix])
                        {
                            break;
                        }
                        if (ix == 3)
                        {
                            count++;
                        }
                    }
                }
                //can go up left
                if (y >= 3)
                {
                    for (int ix = 0; ix <= 3; ix++)
                    {
                        if (chars[y - ix][x - ix] != XMAS[ix])
                        {
                            break;
                        }
                        if (ix == 3)
                        {
                            count++;
                        }
                    }
                }
            }
            //can go down
            if (y <= chars.Length - 4)
            {
                for (int ix = 0; ix <= 3; ix++)
                {
                    if (chars[y + ix][x] != XMAS[ix])
                    {
                        break;
                    }
                    if (ix == 3)
                    {
                        count++;
                    }
                }
            }
            //can go up
            if (y >= 3)
            {
                for (int ix = 0; ix <= 3; ix++)
                {
                    if (chars[y - ix][x] != XMAS[ix])
                    {
                        break;
                    }
                    if (ix == 3)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //add masks and mask manipulation to aoclib
            //Solve
            long result = 0;
            char[][] chars = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                chars[i] = new char[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    chars[i][j] = lines[i][j];
                }
            }
            for (int i = 1; i < lines.Length-1; i++)
            {
                for (int j = 1; j < lines[i].Length-1; j++)
                {
                    if (chars[i][j] == 'A')
                    {
                        if (isX(chars, j, i)) result++;
                    }
                }
            }

            return result;
        }
    }
}

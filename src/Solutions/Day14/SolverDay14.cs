using TillSharp.Math.Array;
using AOCLib;

namespace Solutions
{
    public class SolverDay14 : ISolver
    {
        public class Robot
        {
            public long x;
            public long y;
            public long dx;
            public long dy;
            public long maxX;
            public long maxY;

            public Robot(long x, long y, long dx, long dy, long maxX, long maxY)
            {
                this.x = x;
                this.y = y;
                this.dx = dx;
                this.dy = dy;
                this.maxX = maxX;
                this.maxY = maxY;
            }

            public void Move()
            {
                this.x = (x + dx).mod(maxX);
                this.y = (y + dy).mod(maxY);
            }
            
            //negativ % needs this:
            long mod(long x, long m)
            {
                return (x % m + m) % m;
            }

            public int Quadrant()
            {
                if(this.x > this.maxX / 2)
                {
                    if(this.y > this.maxY / 2)
                    {
                        return 1;
                    }
                    else if(this.y < this.maxY / 2)
                    {
                        return 2;
                    }
                }
                else if (this.x < this.maxX / 2)
                {
                    if (this.y > this.maxY / 2)
                    {
                        return 3;
                    }
                    else if (this.y < this.maxY / 2)
                    {
                        return 4;
                    }
                }
                return 0;
            }
        }

        public long SolvePart1(string[] lines, string text)
        {
            //Solve
            long result = 0;

            List<Robot> robots = new();
            
            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                robots.Add(new Robot(nums[0], nums[1], nums[2], nums[3], 101, 103));
            }

            for (int i = 0; i < 100; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move();
                }
            }

            var quadrants = new int[5];
            foreach (var robot in robots)
            {
                quadrants[robot.Quadrant()]++;
            }

            result = quadrants[1] * quadrants[3] * quadrants[2] * quadrants[4];

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            List<Robot> robots = new();

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                robots.Add(new Robot(nums[0], nums[1], nums[2], nums[3], 101, 103));
            }

            for (int i = 0; i < 1000000; i++)
            {
                int[,] map = new int[103,101];
                bool print = true; //i guess all robots are unique pixels
                foreach (var robot in robots)
                {
                    robot.Move();
                    if (map[robot.y, robot.x] == 1) print = false;
                    map[robot.y, robot.x] = 1;
                }
                if(print)
                {
                    map.Print();
                    return i; //seems to be the answer, but is it always the first?
                }
            }

            return result;
        }
    }
}

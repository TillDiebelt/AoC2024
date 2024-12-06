namespace Solutions
{
    public class SolverDay06 : ISolver
    {
        //numbering is important -> rotate right is +1 % 4
        public enum Rotation
        {
            UP = 0,
            RIGHT = 1, 
            DOWN = 2, 
            LEFT = 3
        }

        public class Guard
        {
            private int posX = 0;
            private int posY = 0;
            private List<Move> moves;

            private Rotation Rotation { get; set; }

            public Guard(int x, int y, Rotation rotation) 
            { 
                this.posX = x;
                this.posY = y;
                this.Rotation = rotation;
                //order is important for the rotation (same index)
                moves = new List<Move>()
                {
                    MoveUp,
                    MoveRight,
                    MoveDown,
                    MoveLeft
                };
            }

            private delegate bool Move(int[,] field);
            private bool MoveUp(int[,] field)
            {
                if (field[posY - 1, posX] == 0)
                {
                    this.posY -= 1;
                    this.Rotation = Rotation.UP;
                    return true;
                }
                return false;
            }
            private bool MoveDown(int[,] field)
            {
                if (field[posY + 1, posX] == 0)
                {
                    this.posY += 1;
                    this.Rotation = Rotation.DOWN;
                    return true;
                }
                return false;
            }
            private bool MoveLeft(int[,] field)
            {
                if (field[posY, posX - 1] == 0)
                {
                    this.posX -= 1;
                    this.Rotation = Rotation.LEFT;
                    return true;
                }
                return false;
            }
            private bool MoveRight(int[,] field)
            {
                if (field[posY, posX + 1] == 0)
                {
                    this.posX += 1;
                    this.Rotation = Rotation.RIGHT;
                    return true;
                }
                return false;
            }

            private void RotateRight()
            {
                this.Rotation = (Rotation)(((int)this.Rotation + 1) % 4);
            }

            public (int x, int y, Rotation r) Visit(int[,] field)
            {
                while (!this.moves[(int)this.Rotation](field))
                {
                    this.RotateRight();
                }
                return (this.posX, this.posY, this.Rotation);
            }
        }


        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;
            int[,] field = new int[lines.Length, lines[0].Length];
            int[,] visits = new int[lines.Length, lines[0].Length];
            Guard guard = null;
            for (int i = 0; i < lines.Length; i++)
            {
               for(int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '.')
                    {
                        field[i, j] = 0;
                    }
                    else if (lines[i][j] == '#')
                    {
                        field[i, j] = 1;
                    }
                    else if (lines[i][j] == '^')
                    {
                        field[i, j] = 0;
                        guard = new Guard(j, i, Rotation.UP);
                        visits[i, j] = 1;
                        result++;
                    }

                }
            }

            bool running = true;
            while (running) 
            {
                (int x, int y, Rotation r) guardPos = guard.Visit(field);
                if (visits[guardPos.y, guardPos.x] == 0)
                {
                    result++;
                }
                visits[guardPos.y, guardPos.x]++;
                if (guardPos.y == 0 || guardPos.y == lines.Length-1 || guardPos.x == 0 || guardPos.x == lines[0].Length-1) 
                {
                    running = false;
                }
            }
          
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            (int x, int y, Rotation r) guard = (0, 0, Rotation.UP);
            int[,] field = new int[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '.')
                    {
                        field[i, j] = 0;
                    }
                    else if (lines[i][j] == '#')
                    {
                        field[i, j] = 1;
                    }
                    else if (lines[i][j] == '^')
                    {
                        field[i, j] = 0;
                        guard.x = j;
                        guard.y = i;
                        guard.r = Rotation.UP;
                    }

                }
            }

            for(int y = 0; y  < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == 1) { continue; }
                    if(y == guard.y && x == guard.x) { continue; }
                    result += loops(field, guard, x, y, lines[0].Length, lines.Length) ? 1 : 0;
                }
            }

            return result;
        }

        public static bool loops(int[,] useField, (int x, int y, Rotation r) guardInitPos, int x, int y, int lenX, int lenY)
        {
            int[,] field = useField.Clone() as int[,];
            Guard guard = new Guard(guardInitPos.x, guardInitPos.y, guardInitPos.r);
            bool running = true;
            HashSet<(int, int, Rotation)> visits = new();
            visits.Add((guardInitPos.x, guardInitPos.y, guardInitPos.r));
            field[y, x] = 1; //set the additional stone
            while (running)
            {
                (int x, int y, Rotation r) guardPos = guard.Visit(field);
                if (visits.Contains(guardPos))
                {
                    return true; //it loops!
                }
                visits.Add(guardPos);
                if (guardPos.y == 0 || guardPos.y == lenY - 1 || guardPos.x == 0 || guardPos.x == lenX - 1)
                {
                    return false; //he escaped :(
                }
            }
            return false;
        }
    }
}

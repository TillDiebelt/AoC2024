using AOCLib;
using System.IO;
using System.Numerics;

namespace Solutions
{
    public class Game
    {
        public (long x, long y) ButtonA;
        public (long x, long y) ButtonB;
        public (long x, long y) Goal;

        public Game((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) goal)
        {
            ButtonA = buttonA;
            ButtonB = buttonB;
            Goal = goal;
        }

        public long Play()
        {
            //brute force, try with least presses of b so b first, yeaah greed
            long result = 0;

            for(int b = 0; b < 100; b++)
            {
                for (int a = 0; a < 100; a++)
                {
                    if(b* ButtonB.y + a * ButtonA.y == Goal.y && b * ButtonB.x + a * ButtonA.x == Goal.x)
                    {
                        return a * 3 + b;
                    }
                }
            }

            return result;
        }

        public long PlayP2()
        {
            long result = 0;
            long offset = 10000000000000;
            /*
             solve the equations for minumum solution:
                ButtonA.x * a + ButtonB.x * b = Goal.x
                ButtonA.y * a + ButtonB.y * b = Goal.y
                with min 3*a + b
             */

            /*
             solution as vectors with matrix (greedy approach, but seems to work):
             AB
             CD
             
             determinant = 1/(A*D - B*C) (we only need long so invert it)
             */
            long determinant = ButtonA.x * ButtonB.y - ButtonA.y * ButtonB.x;
            if (determinant == 0)
            {
                return 0;
            }

            //Cramers rule, needed to google that one, tf I would still know that off my head
            long determinantA = (Goal.x + offset) * ButtonB.y - (Goal.y + offset) * ButtonB.x;
            long determinantB = ButtonA.x * (Goal.y + offset) - ButtonA.y * (Goal.x + offset);

            //we calc with long not double so check if we can divide, otherwise solution has fractions of a press
            if (determinantA % determinant != 0 || determinantB % determinant != 0)
            {
                return 0;
            }

            long pressesA = determinantA / determinant;
            long pressesB = determinantB / determinant;

            result = 3 * pressesA + pressesB;

            return result;
        }
    }

    public class SolverDay13 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;

            List<Game> games = new List<Game>();

            for (int i = 0; i < lines.Length; i+=3)
            {
                if (lines[i].Length == 0) i++;
                var buttonA = lines[i].GetNumbers();
                var bAf = (buttonA[0], buttonA[1]);

                var buttonB = lines[i + 1].GetNumbers();
                var bBf = (buttonB[0], buttonB[1]);

                var goal = lines[i + 2].GetNumbers();
                var goalF = (goal[0], goal[1]);

                games.Add(new Game(bAf, bBf, goalF));
            }

            foreach (var game in games)
            {
                result += game.Play();
            }

            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            List<Game> games = new List<Game>();

            for (int i = 0; i < lines.Length; i += 3)
            {
                if (lines[i].Length == 0) i++;
                var buttonA = lines[i].GetNumbers();
                var bAf = (buttonA[0], buttonA[1]);

                var buttonB = lines[i + 1].GetNumbers();
                var bBf = (buttonB[0], buttonB[1]);

                var goal = lines[i + 2].GetNumbers();
                var goalF = (goal[0], goal[1]);

                games.Add(new Game(bAf, bBf, goalF));
            }

            foreach (var game in games)
            {
                result += (long)game.PlayP2();
            }

            return result;
        }
    }
}

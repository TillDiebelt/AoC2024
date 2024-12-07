using AOCLib;

namespace Solutions
{
    public class SolverDay07 : ISolver
    {     
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                long first = nums[0];
                var others = nums.Skip(1).ToList();
                if (trySolve(others, first, new List<string>() { "+", "*" }))
                    result += first;
            }
           
            return result;
        }

        public List<string> perms(long size, List<string> operators)
        {
            List<string> perms = operators;
            for (int i = 0; i < size; i++)
            {
                List<string> perms2 = new List<string>();
                foreach(var p in perms)
                {
                    foreach(var o in operators)
                    {
                        perms2.Add(p + o);
                    }
                }
                perms = perms2;
            }
            return perms;
        }

        public bool trySolve(List<long> nums, long res, List<string> operators)
        {
            var solvers = perms(nums.Count - 2, operators);
            foreach (var s in solvers)
            {
                long result = nums[0];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '+')
                    {
                        result += nums[i + 1];
                    }
                    else if (s[i] == '*')
                    {
                        result *= nums[i + 1];
                    }
                    else if (s[i] == '|')
                    {
                        result = long.Parse(result + "" + nums[i + 1]);
                    }
                }
                if (result == res)
                    return true;
            }
            return false;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var nums = lines[i].GetNumbers();
                long first = nums[0];
                var others = nums.Skip(1).ToList();
                if (trySolve(others, first, new List<string>() { "+", "*", "|" }))
                    result += first;
            }

            return result;
        }
    }
}

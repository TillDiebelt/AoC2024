using AOCLib;

namespace Solutions
{
    public class SolverDay05 : ISolver
    {
        class Printable: IComparable<Printable>
        {
            public long value;
            public List<long> after = new();

            public int CompareTo(Printable? other)
            {
                if(this.after.Contains(other.value))
                {
                    return -1;
                }
                else
                    return 1;
            }

            public bool isValid(IEnumerable<long> afters)
            {
                foreach(var i in afters)
                {
                    if (!after.Contains(i)) return false;

                }
                return true;
            }

        }
        public long SolvePart1(string[] lines, string text)
        {
            bool s = false;
            Dictionary<long, Printable> pages = new Dictionary<long, Printable>();
            List<List<long>> prints = new List<List<long>>();
            //Solve
            long result = 0;
            for (long i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    s = true;
                    continue;
                }

                if (!s)
                {
                    var numbers = lines[i].GetNumbers();
                    long left = numbers.First();
                    long right = numbers.Last();
                    if (pages.ContainsKey(left))
                    {
                        pages[left].after.Add(right);
                    }
                    else
                    {
                        pages.Add(left, new Printable() { value = left });
                        pages[left].after.Add(right);
                    }
                }
                else
                {
                    prints.Add(lines[i].GetNumbers());
                }
            }

            foreach(var p in prints)
            {
                bool valid = true;
                for(int i = 0; i < p.Count-1; i++) 
                {
                    if (pages.ContainsKey(p[i]))
                        valid &= pages[p[i]].isValid(p.Skip(i + 1));
                    else
                        valid = false;
                }
                if(valid)
                {
                    result += p[p.Count/2];
                }
            }
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {

            bool s = false;
            Dictionary<long, Printable> pages = new Dictionary<long, Printable>();
            List<List<long>> prints = new List<List<long>>();
            //Solve
            long result = 0;
            for (long i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    s = true;
                    continue;
                }

                if (!s)
                {
                    var numbers = lines[i].GetNumbers();
                    long left = numbers.First();
                    long right = numbers.Last();
                    if (pages.ContainsKey(left))
                    {
                        pages[left].after.Add(right);
                    }
                    else
                    {
                        pages.Add(left, new Printable() { value = left });
                        pages[left].after.Add(right);
                    }
                }
                else
                {
                    prints.Add(lines[i].GetNumbers());
                }
            }

            foreach (var p in prints)
            {
                bool valid = true;
                for (int i = 0; i < p.Count - 1; i++)
                {
                    if (pages.ContainsKey(p[i]))
                    {
                        valid &= pages[p[i]].isValid(p.Skip(i + 1));
                    }
                    else
                    {
                        pages.Add(p[i], new Printable() { value = p[i] });
                        valid = false;
                    }
                }
                if (!valid)
                {
                    var tmp = p.Select(x => pages[x]).ToList();
                    tmp.Sort();
                    result += tmp[tmp.Count / 2].value;
                }
            }
            return result;
        }
    }
}

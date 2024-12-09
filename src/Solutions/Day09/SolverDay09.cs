using AOCLib;

namespace Solutions
{
    public class SolverDay09 : ISolver
    {
        public long SolvePart1(string[] lines, string text)
        {            
            //Solve
            long result = 0;
            List<((int start, int end) range, long id)> data = new();
            List<(int start, int end)> free = new();
            long id = 0;
            int currentIndex = 0;
            for (int i = 0; i < lines[0].Length; i++)
            {
                int value = int.Parse(lines[0][i]+"");
                if (i % 2 == 0)
                {
                    if(value > 0)
                        data.Add((range: (start: currentIndex, end: currentIndex+value-1), id: id));
                    id++;
                    currentIndex += value;
                }
                else
                {
                    if (value > 0)
                        free.Add((start: currentIndex, end: currentIndex + value - 1));
                    currentIndex += value;
                }
            }

            for(int i = data.Count - 1; i >= 0; i-- )
            {
                if(free.Count == 0)
                {
                    break;
                }
                int dataLength = data[i].range.end - data[i].range.start + 1;
                for (int j = 0; j < free.Count; j++)
                {
                    if (free.Count == 0 || free[0].start >= data[i].range.end)
                    {
                        break;
                    }
                    int freeLength = free[j].end - free[j].start + 1;
                    if (dataLength <= freeLength)
                    {
                        data.Add((range: (start: free[j].start, end: free[j].start + dataLength - 1), id: data[i].id));
                        free[j] = (start: free[j].start+dataLength, end: free[j].end);
                        if (dataLength - freeLength == 0)
                        {
                            free.RemoveAt(j);
                        }
                        dataLength -= freeLength;
                        data.RemoveAt(i);
                        break;
                    }
                    else if (dataLength > freeLength)
                    {
                        data.Add((range: (start: free[j].start, end: free[j].end), id: data[i].id));
                        data[i] = (range: (start: data[i].range.start, end: data[i].range.end - freeLength), id: data[i].id);
                        free.RemoveAt(j);
                        dataLength -= freeLength;
                        j--;
                    }
                }
            }

            foreach (var d in data)
            {
                result += Utils.GaussSum(d.range.end, d.range.start-1) * d.id;
            }
          
            return result;
        }

        public long SolvePart2(string[] lines, string text)
        {
            //Solve
            long result = 0;
            List<((int start, int end) range, long id)> data = new();
            List<(int start, int end)> free = new();
            long id = 0;
            int currentIndex = 0;
            for (int i = 0; i < lines[0].Length; i++)
            {
                int value = int.Parse(lines[0][i] + "");
                if (i % 2 == 0)
                {
                    if (value > 0)
                        data.Add((range: (start: currentIndex, end: currentIndex + value - 1), id: id));
                    id++;
                    currentIndex += value;
                }
                else
                {
                    if (value > 0)
                        free.Add((start: currentIndex, end: currentIndex + value - 1));
                    currentIndex += value;
                }
            }

            for (int i = data.Count - 1; i >= 0; i--)
            {
                int dataLength = data[i].range.end - data[i].range.start + 1;
                if (free.Any(x => x.end - x.start + 1 >= dataLength && x.end < data[i].range.start))
                {
                    var freedex = free.Where(x => x.end - x.start + 1 >= dataLength && x.end < data[i].range.start).First();
                    int j = free.IndexOf(freedex);
                    int freeLength = free[j].end - free[j].start + 1;
                    data[i] = ((range: (start: free[j].start, end: free[j].start + dataLength - 1), id: data[i].id));
                    free[j] = (start: free[j].start + dataLength, end: free[j].end);
                    if (dataLength - freeLength == 0)
                    {
                        free.RemoveAt(j);
                    }
                }
            }

            foreach (var d in data)
            {
                result += Utils.GaussSum(d.range.end, d.range.start - 1) * d.id;
            }

            return result;
        }
    }
}

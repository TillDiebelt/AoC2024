namespace AOCLib
{
    public static class Pathfinder<T,S>
    {        
        public static List<S> FindPath(
            T Map,
            Func<T, IEnumerable<S>> GetStarts,
            Func<T, S, bool> IsGoal,
            Func<T, S, double> Distance,
            Func<T, S, S, double> StepCost,
            Func<T, S, IEnumerable<S>> GetSuccessors
            )
        {
            Dictionary<S,S> walk = new Dictionary<S,S>();
            Dictionary<S, double> cost = new Dictionary<S, double>();
            var Starts = GetStarts(Map).ToList();
            if (Starts.Count == 0) throw new Exception("no start found");
            var queue = new PriorityQueue<S, double>();
            foreach(var start in Starts)
            {
                queue.Enqueue(start, 0);
                cost[start] = 0;
            }
            S current = Starts[0];
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                double currentCost = cost[current];
                if (IsGoal(Map, current))
                {
                    break;
                }
                IEnumerable<S> successors;
                successors = GetSuccessors(Map, current);
                foreach (var successor in successors)
                {
                    double moveCost = cost[current] + StepCost(Map, current, successor);
                    if (cost.ContainsKey(successor))
                    {
                        if (cost[successor] <= moveCost)
                            continue;
                        else
                        {
                            walk[successor] = current;
                            cost[successor] = moveCost;
                        }
                    }
                    else
                    {
                        walk.Add(successor, current);
                        cost.Add(successor, moveCost);
                    }
                    queue.Enqueue(successor, moveCost + Distance(Map, successor));
                }
            }
            if (!IsGoal(Map, current)) throw new Exception("no path found");

            var path = new List<S>();
            while (!Starts.Contains(current))
            {
                path.Add(current);
                current = walk[current];
            }
            path.Reverse();
            return path;
        }
    }    
}

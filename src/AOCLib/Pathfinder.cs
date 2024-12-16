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

        public static List<List<S>> FindAllPaths(
            T Map,
            Func<T, IEnumerable<S>> GetStarts,
            Func<T, S, bool> IsGoal,
            Func<T, S, double> Distance,
            Func<T, S, S, double> StepCost,
            Func<T, S, IEnumerable<S>> GetSuccessors
            )
        {
            Dictionary<S, HashSet<S>> walk = new Dictionary<S, HashSet<S>>();
            Dictionary<S, double> cost = new Dictionary<S, double>();
            var Starts = GetStarts(Map).ToList();
            if (Starts.Count == 0) throw new Exception("no start found");
            var queue = new PriorityQueue<S, double>();
            foreach (var start in Starts)
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
                        if (cost[successor] < moveCost)
                            continue; 
                        else if (cost[successor] == moveCost)
                        {
                            walk[successor].Add(current);
                        }
                        else
                        {
                            walk[successor] = new HashSet<S>() { current };
                            cost[successor] = moveCost;
                        }
                    }
                    else
                    {
                        walk.Add(successor, new HashSet<S>() { current });
                        cost.Add(successor, moveCost);
                    }
                    queue.Enqueue(successor, moveCost + Distance(Map, successor));
                }
            }
            if (!IsGoal(Map, current)) throw new Exception("no path found");

            var foo = walk.OrderByDescending(x => x.Value.Count).ToList();
            var paths = GetPaths(ref Starts, current, walk);
            foreach(var p in paths)
            {
                p.Reverse();
            }
            return paths;
        }

        private static List<List<S>> GetPaths(ref List<S> Goals, S current, Dictionary<S, HashSet<S>> walk)
        {
            var paths = new List<List<S>>();
            if (Goals.Contains(current))
            {
                paths.Add(new List<S>() { current });
                return paths;
            }
            foreach (var parent in walk[current])
            {
                var parentPaths = GetPaths(ref Goals, parent, walk);
                foreach(var pp in parentPaths)
                {
                    List<S> path = new();
                    path.AddRange(pp);
                    path.Add(current);
                    paths.Add(path);
                }
            }
            return paths;
        }
    }
}

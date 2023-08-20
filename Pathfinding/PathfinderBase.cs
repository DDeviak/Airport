namespace Pathfinding
{
    public abstract class PathfinderBase<TGraphNode, TGraphArc>
        where TGraphNode : IComparable
        where TGraphArc : class, IArc<TGraphNode>
    {
        protected record struct Flags(double Distance, TGraphArc? PreviousArc, bool IsMarked);

        protected IGraphProvider<TGraphNode, TGraphArc> graph;

        protected PathfinderBase(IGraphProvider<TGraphNode, TGraphArc> graph)
        {
            this.graph = graph;
        }

        protected Dictionary<TGraphNode, Flags> Dijkstra(TGraphNode from, Func<TGraphArc?, TGraphArc, bool> additionalCriteria)
        {
            Dictionary<TGraphNode, Flags> flags = new Dictionary<TGraphNode, Flags>();

            foreach (TGraphNode t in graph.GetNodes())
            {
                flags[t] = new Flags(double.PositiveInfinity, null, false);
            }
            flags[from] = new Flags(0, null, false);

            PriorityQueue<TGraphNode, double> pq = new PriorityQueue<TGraphNode, double>();

            pq.Enqueue(from, 0);

            TGraphNode currentNode;
            while (pq.Count > 0)
            {
                currentNode = pq.Dequeue();
                if (flags[currentNode].IsMarked) continue;
                Flags cf = flags[currentNode];
                List<TGraphArc> OutcomingArcs = graph.GetOutcomingArcs(currentNode).ToList();
                OutcomingArcs.ForEach((TGraphArc t) =>
                {
                    if (!additionalCriteria(cf.PreviousArc, t)) return;
                    if (t.Length + cf.Distance < flags[t.To].Distance)
                    {
                        Flags f = flags[t.To] with
                        {
                            Distance = t.Length + cf.Distance,
                            PreviousArc = t
                        };
                        flags[t.To] = f;
                        pq.Enqueue(t.To, f.Distance);
                    }
                });
                flags[currentNode] = cf with { IsMarked = true };
            }

            return flags;
        }

        protected IEnumerable<TGraphArc>? MakePath(Dictionary<TGraphNode, Flags> flags, TGraphNode from, TGraphNode to)
        {
            List<TGraphArc> path = new List<TGraphArc>();

            if (!flags.ContainsKey(to)) return null;

            TGraphNode currentGraphNode = to;
            while (!currentGraphNode.Equals(from))
            {
                Flags f = flags[currentGraphNode];
                if (f.PreviousArc == null) return null;
                path.Add(f.PreviousArc);
                currentGraphNode = f.PreviousArc.From;
            }

            path.Reverse();
            return path;
        }
    }
}

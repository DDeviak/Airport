using Airport;

namespace Pathfinding
{
    public abstract class PathfinderBase<GraphNodeType, GraphArcType>
        where GraphNodeType : IComparable
        where GraphArcType : class, IArc<GraphNodeType>
    {
        protected record struct Flags(double Distance, GraphArcType? PreviousArc, bool IsMarked);

        public IGraphProvider<GraphNodeType, GraphArcType> Graph;

        public PathfinderBase(IGraphProvider<GraphNodeType, GraphArcType> graph)
        {
            Graph = graph;
        }

        protected Dictionary<GraphNodeType, Flags> Dijkstra(GraphNodeType from, Func<GraphArcType?, GraphArcType, bool> AdditionalCriteria)
        {
            Dictionary<GraphNodeType, Flags> flags = new Dictionary<GraphNodeType, Flags>();

            foreach (GraphNodeType t in Graph.GetNodes())
            {
                flags[t] = new Flags(double.PositiveInfinity, null, false);
            }
            flags[from] = new Flags(0, null, false);

            PriorityQueue<GraphNodeType, double> pq = new PriorityQueue<GraphNodeType, double>();

            pq.Enqueue(from, 0);

            GraphNodeType currentNode;
            while (pq.Count > 0)
            {
                currentNode = pq.Dequeue();
                if (flags[currentNode].IsMarked) continue;
                Flags cf = flags[currentNode];
                List<GraphArcType> OutcomingArc = Graph.GetOutcomingArcs(currentNode).ToList();
                OutcomingArc.ForEach((GraphArcType t) =>
                {
                    if (!AdditionalCriteria(cf.PreviousArc, t)) return;
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

        protected IEnumerable<GraphArcType>? MakePath(Dictionary<GraphNodeType, Flags> flags, GraphNodeType from, GraphNodeType to)
        {
            List<GraphArcType> path = new List<GraphArcType>();

            GraphNodeType currentGraphNode = to;
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

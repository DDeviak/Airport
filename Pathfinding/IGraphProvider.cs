namespace Pathfinding
{
    public interface IGraphProvider<NodeType, ArcType>
        where NodeType : IComparable
        where ArcType : class, IArc<NodeType>
    {
        public IEnumerable<NodeType> GetNodes();

        public IEnumerable<ArcType> GetOutcomingArcs(NodeType node);
    }
}

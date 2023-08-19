namespace Pathfinding
{
    public interface IArc<NodeType>
        where NodeType : IComparable
    {
        public double Length { get; }
        public NodeType From { get; }
        public NodeType To { get; }
    }
}

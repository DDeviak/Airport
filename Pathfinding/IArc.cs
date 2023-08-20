namespace Pathfinding
{
    public interface IArc<out TNode>
        where TNode : IComparable
    {
        public double Length { get; }

        public TNode From { get; }

        public TNode To { get; }
    }
}

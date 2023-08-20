namespace Pathfinding
{
    public interface IGraphProvider<TNode, TArc>
        where TNode : IComparable
        where TArc : class, IArc<TNode>
    {
        public IEnumerable<TNode> GetNodes();

        public IEnumerable<TArc> GetOutcomingArcs(TNode node);
    }
}

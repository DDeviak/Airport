// <copyright file="IGraphProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Pathfinding
{
	/// <summary>
	/// Provides generalized type fore graph storing.
	/// </summary>
	/// <typeparam name="TNode">Graph node type.</typeparam>
	/// <typeparam name="TArc">Graph arc type.</typeparam>
	public interface IGraphProvider<TNode, TArc>
		where TNode : IComparable
		where TArc : class, IArc<TNode>
	{
		/// <summary>
		/// Gets all nodes of the graph.
		/// </summary>
		/// <returns><see cref="IEnumerable{TNode}" /> of Nodes.</returns>
		public IEnumerable<TNode> GetNodes();

		/// <summary>
		/// Gets all arcs that start in specific node.
		/// </summary>
		/// <param name="node">Arcs starting node.</param>
		/// <returns><see cref="IEnumerable{TArc}" /> of outcoming Arcs. Empty if none is present in graph.</returns>
		public IEnumerable<TArc> GetOutcomingArcs(TNode node);
	}
}

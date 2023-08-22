// <copyright file="IArc.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Pathfinding
{
	/// <summary>
	/// Provides generalized graph arc type.
	/// </summary>
	/// <typeparam name="TNode">Graph node type.</typeparam>
	public interface IArc<out TNode>
		where TNode : IComparable
	{
		/// <summary>
		/// Gets length of provided arc.
		/// </summary>
		public double Length { get; }

		/// <summary>
		/// Gets arc`s starting node.
		/// </summary>
		public TNode From { get; }

		/// <summary>
		/// Gets arc`s ending node.
		/// </summary>
		public TNode To { get; }
	}
}

using System.Collections.Generic;

namespace Arilon.Core.Abstractions
{
	/// <summary>
	/// Represents a path between two <see cref="INode"/> s
	/// </summary>
	public interface IPath
	{
		/// <summary>
		/// The <see cref="INode"/> s and <see cref="IEdge"/> s forming the relationship. Including
		/// the start node and end node
		/// </summary>
		IEnumerable<IGraphObject> NodesAndEdges { get; }
	}
}
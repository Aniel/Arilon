using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	/// <summary>
	/// Represents a path between two <see cref="INode"/> s
	/// </summary>
	public class Path : IPath
	{
		/// <summary>
		/// Creates an instance of <see cref="Path"/>
		/// </summary>
		/// <param name="nodesAndEdges">
		/// The nodes and edges that make the path. Including the start node and the end node
		/// </param>
		public Path(IEnumerable<IGraphObject> nodesAndEdges)
		{
			NodesAndEdges = nodesAndEdges;
		}

		/// <summary>
		/// The <see cref="INode"/> s and <see cref="IEdge"/> s forming the relationship. Including
		/// the start node and end node
		/// </summary>
		public IEnumerable<IGraphObject> NodesAndEdges { get; internal protected set; }
	}
}
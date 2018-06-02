using System;
using System.Collections.Generic;
using System.Text;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	/// <summary>
	/// Represents a path in an <see cref="ArilonResult"/>
	/// </summary>
	public class ArilonResultPath : Path
	{
		/// <summary>
		/// Creates an instance of <see cref="ArilonResultPath"/>
		/// </summary>
		/// <param name="identifier">   The <see cref="Identifier"/> that represents the Path</param>
		/// <param name="nodesAndEdges">
		/// The nodes and edges that make the path. Including the start node and the end node
		/// </param>
		public ArilonResultPath(string identifier, IEnumerable<IGraphObject> nodesAndEdges) : base(nodesAndEdges)
		{
			Identifier = identifier;
		}

		/// <summary>
		/// The identifier representing the relationship between the two <see cref="INode"/> s
		/// </summary>
		public string Identifier { get; protected internal set; }
	}
}
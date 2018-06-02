using System.Collections.Generic;

namespace Arilon.Core.Abstractions
{
	/// <summary>
	/// Represents a node in a graph
	/// </summary>
	public interface INode : IGraphObject
	{
		/// <summary>
		/// The ids of the incomming edges
		/// </summary>
		List<string> IncomingEdgeIds { get; }

		/// <summary>
		/// The ids of the outgoing edges
		/// </summary>
		List<string> OutgoingEdgeIds { get; }
	}
}
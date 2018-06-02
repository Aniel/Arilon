using System;
using System.Collections.Generic;
using System.Text;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	/// <summary>
	/// Represents a node in a graph
	/// </summary>
	public class Node : INode
	{
		/// <summary>
		/// The id of the object
		/// </summary>
		public string Id { get; protected set; }

		/// <summary>
		/// The ids of the incomming edges
		/// </summary>
		public List<string> IncomingEdgeIds { get; protected set; }

		/// <summary>
		/// The ids of the outgoing edges
		/// </summary>
		public List<string> OutgoingEdgeIds { get; protected set; }

		/// <summary>
		/// The properties of the object
		/// </summary>
		public Dictionary<string, string> Properties { get; protected set; }

		/// <summary>
		/// Initialize an instance of <see cref="Node"/>
		/// </summary>
		/// <param name="id">The id of the node</param>
		/// <param name="incomingEdgeIds">The ids of incoming edges</param>
		/// <param name="outgoingEdgeIds">The ids of outgoing edges</param>
		/// <param name="properties">The properties of the node</param>
		public Node(string id, List<string> incomingEdgeIds = null, List<string> outgoingEdgeIds = null, Dictionary<string, string> properties = null)
		{
			Id = id;
			IncomingEdgeIds = incomingEdgeIds ?? new List<string>();
			OutgoingEdgeIds = outgoingEdgeIds ?? new List<string>();
			Properties = properties ?? new Dictionary<string, string>();
		}
	}
}
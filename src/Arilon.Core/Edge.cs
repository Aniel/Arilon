using System;
using System.Collections.Generic;
using System.Text;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	/// <summary>
	/// Represents an edge in a graph
	/// </summary>
	public class Edge : IEdge
	{
		/// <summary>
		/// The id of the node at the start of the edge
		/// </summary>
		public string FromNodeId { get; protected set; }

		/// <summary>
		/// The unique id of the edge in the graph
		/// </summary>
		public string Id { get; protected set; }

		/// <summary>
		/// The properties in the graph
		/// </summary>
		public Dictionary<string, string> Properties { get; protected set; }

		/// <summary>
		/// The id of the node at the end of the edge
		/// </summary>
		public string ToNodeId { get; protected set; }

		/// <summary>
		/// Initialize an instance of <see cref="Edge"/>
		/// </summary>
		/// <param name="id">The id of the edge</param>
		/// <param name="fromNodeId">The id of the node at the start of the edge</param>
		/// <param name="toNodeId">The id of the node at the end of the edge</param>
		/// <param name="properties">The properties of the edge</param>
		public Edge(string id, string fromNodeId, string toNodeId, Dictionary<string, string> properties = default)
		{
			Id = id;
			FromNodeId = fromNodeId;
			ToNodeId = toNodeId;
			Properties = properties ?? new Dictionary<string, string>();
		}
	}
}
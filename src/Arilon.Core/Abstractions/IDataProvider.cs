using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arilon.Core.Abstractions
{
	/// <summary>
	/// Represents a data provider to calculate an relationship between two nodes in a graph
	/// </summary>
	public interface IDataProvider
	{
		/// <summary>
		/// Loads a edge. If the edge with the given id was not found it will return null
		/// </summary>
		/// <param name="id">The id of the edge</param>
		/// <returns>The edge or null if not found</returns>
		Task<IEdge> GetEdge(string id);

		/// <summary>
		/// Loads a node. If the node with the given id was not found it will return null
		/// </summary>
		/// <param name="id">The id of the node</param>
		/// <returns>The node or null if not found</returns>
		Task<INode> GetNode(string id);

		/// <summary>
		/// Returns a list of the shortest paths between two nodes. Includes the start and end node.
		/// </summary>
		/// <param name="fromNodeID">The id of the start node</param>
		/// <param name="toNodeID">  The id of the end node</param>
		/// <returns>A list of paths between the start and end node</returns>
		Task<IEnumerable<IPath>> ShortestPaths(string fromNodeID, string toNodeID);

		/// <summary>
		/// Returns a list of the shortest paths between two nodes. Includes the start and end node.
		/// </summary>
		/// <param name="fromNode">The start node</param>
		/// <param name="toNode">  The end node</param>
		/// <returns>A list of paths between the start and end node</returns>
		Task<IEnumerable<IPath>> ShortestPaths(INode fromNode, INode toNode);
	}
}
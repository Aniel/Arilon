using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arilon.Core.Abstractions
{
	public interface IArilon
	{
		/// <summary>
		/// The data provider that provides data for the calculation
		/// </summary>
		IDataProvider DataProvider { get; set; }

		/// <summary>
		/// The names of properties on edges whose values will be included in the identifier.
		/// </summary>
		IEnumerable<string> IncludedEdgeProperties { get; set; }

		/// <summary>
		/// The names of properties on nodes whose values will be included in the identifier.
		/// </summary>
		IEnumerable<string> IncludedNodeProperties { get; set; }

		/// <summary>
		/// Calculates an identifier that describes the relationship between two nodes
		/// </summary>
		/// <param name="fromNodeId">The id of the start node</param>
		/// <param name="toNodeId">  The id of the end node</param>
		/// <returns>The identifier and paths that represents the two nodes</returns>
		Task<ArilonResult> CalculateRelationIdentifier(string fromNodeId, string toNodeId);

		/// <summary>
		/// Calculates an identifier that describes the relationship between two nodes
		/// </summary>
		/// <param name="fromNode">The start node</param>
		/// <param name="toNode">  The end node</param>
		/// <returns>The identifier and Paths that represents the two nodes</returns>
		Task<ArilonResult> CalculateRelationIdentifier(INode fromNode, INode toNode);
	}
}
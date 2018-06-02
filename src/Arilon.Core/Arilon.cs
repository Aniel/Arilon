using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	public class Arilon : IArilon
	{
		/// <summary>
		/// Creates an instance of the Arilon class
		/// </summary>
		/// <param name="dataProvider">         The data provider that provides data for the calculation</param>
		/// <param name="includeEdgeProperties">
		/// The edge properties whose values will be included in the result
		/// </param>
		/// <param name="includeNodeProperties">
		/// The node properties whose values will be included in the result
		/// </param>
		public Arilon(IDataProvider dataProvider, IEnumerable<string> includeEdgeProperties = null, IEnumerable<string> includeNodeProperties = null)
		{
			DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
			IncludedEdgeProperties = includeEdgeProperties ?? new List<string>();
			IncludedNodeProperties = includeNodeProperties ?? new List<string>();
		}

		/// <summary>
		/// The data provider that provides data for the calculation
		/// </summary>
		public IDataProvider DataProvider { get; set; }

		/// <summary>
		/// The identifier that will be used in the result relationship string for an edge
		/// </summary>
		public string EdgeIdentifier { get; set; } = "e";

		/// <summary>
		/// The identifier that will be in between nodes and edges when the edge direction is forward
		/// </summary>
		public string ForwardEdgeIdentifier { get; set; } = "->";

		/// <summary>
		/// The names of properties on edges whose values will be included in the identifier.
		/// </summary>
		public IEnumerable<string> IncludedEdgeProperties { get; set; }

		/// <summary>
		/// The names of properties on nodes whose values will be included in the identifier.
		/// </summary>
		public IEnumerable<string> IncludedNodeProperties { get; set; }

		/// <summary>
		/// The identifier that will be used in the result relationship string for an node
		/// </summary>
		public string NodeIdentifier { get; set; } = "n";

		/// <summary>
		/// The identifier that will be used in the result to separate paths
		/// </summary>
		public string PathSeperator { get; set; } = "&";

		public string ReverseEdgeIdentifier { get; set; } = "<-";

		/// <summary>
		/// Calculates an identifier that describes the relation between two nodes
		/// </summary>
		/// <param name="fromNodeId">The id of the start node</param>
		/// <param name="toNodeId">  The id of the end node</param>
		/// <returns>The identifier and paths that represents the two nodes</returns>
		public async Task<ArilonResult> CalculateRelationIdentifier(string fromNodeId, string toNodeId)
		=> await CalculateRelationIdentifier(
			await DataProvider.GetNode(fromNodeId),
			await DataProvider.GetNode(toNodeId));

		/// <summary>
		/// Calculates an identifier that describes the relation between two nodes
		/// </summary>
		/// <param name="fromNode">The start node</param>
		/// <param name="toNode">  The end node</param>
		/// <returns>The identifier and paths that represents the two nodes</returns>
		public async Task<ArilonResult> CalculateRelationIdentifier(INode fromNode, INode toNode)
		{
			var shortestPaths = await DataProvider.ShortestPaths(fromNode, toNode);
			var builder = new StringBuilder();
			var pathsCount = shortestPaths.Count();
			var resultPaths = new List<ArilonResultPath>(pathsCount);
			for (int i = 0; i < pathsCount; i++)
			{
				var prevoiusNodeId = "";
				var path = shortestPaths.ElementAt(i);
				foreach (var item in path.NodesAndEdges)
				{
					switch (item)
					{
						case INode node:
							prevoiusNodeId = node.Id;
							builder.Append(NodeIdentifier);
							builder.Append("(");
							_addProperties(ref builder, node, IncludedNodeProperties);
							builder.Append(")");
							break;

						case IEdge edge:
							if (edge.ToNodeId == prevoiusNodeId)
								builder.Append(ReverseEdgeIdentifier);
							else
								builder.Append(ForwardEdgeIdentifier);
							builder.Append(EdgeIdentifier);
							builder.Append("(");
							_addProperties(ref builder, edge, IncludedEdgeProperties);
							builder.Append(")");
							if (edge.ToNodeId == prevoiusNodeId)
								builder.Append(ReverseEdgeIdentifier);
							else
								builder.Append(ForwardEdgeIdentifier);
							break;
					}
				}
				var resultPath = new ArilonResultPath(builder.ToString(), path.NodesAndEdges);
				resultPaths.Add(resultPath);
				builder.Clear();
			}

			return new ArilonResult(
				string.Join(PathSeperator, resultPaths.Select(x => x.Identifier)),
				fromNode,
				toNode,
				resultPaths);
		}

		private void _addProperties(ref StringBuilder builder, in IGraphObject item, in IEnumerable<string> includedProperties)
		{
			var count = includedProperties.Count();
			for (int i = 0; i < count; i++)
			{
				var property = includedProperties.ElementAt(i);
				builder.Append(property);
				builder.Append(":");
				if (item.Properties != null && item.Properties.TryGetValue(property, out var value))
				{
					builder.Append(value);
				}
				if (count < i - 1)
					builder.Append(",");
			}
		}
	}
}
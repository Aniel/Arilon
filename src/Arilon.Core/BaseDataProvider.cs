using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	public abstract class BaseDataProvider : IDataProvider
	{
		public abstract Task<IEdge> GetEdge(string id);

		public abstract Task<INode> GetNode(string id);

		public async Task<IEnumerable<IPath>> ShortestPaths(string fromNodeID, string toNodeID) => await ShortestPaths(await GetNode(fromNodeID), await GetNode(toNodeID));

		public async Task<IEnumerable<IPath>> ShortestPaths(INode fromNode, INode toNode)
		{
			if (fromNode == null)
				throw new ArgumentNullException(nameof(fromNode), "The given node was null");

			if (toNode == null)
				throw new ArgumentNullException(nameof(toNode), "The given node was null");

			List<IPath> foundPaths = new List<IPath>();
			await getPath(fromNode, toNode, foundPaths, new List<IGraphObject>());

			if (foundPaths.Count == 0)
				return foundPaths;

			var minPathLength = foundPaths.Min(x => x.NodesAndEdges.Count());

			//Faster version of: x => x.NodesAndEdges.Count() <= minPathLength
			return foundPaths.Where(x => !x.NodesAndEdges.Skip(minPathLength).Any());
		}

		protected async Task getPath(INode currentNode, INode targetNode, List<IPath> foundPaths, List<IGraphObject> walkedPath)
		{
			if (targetNode.Id == currentNode.Id)
			{
				walkedPath.Add(targetNode);
				IPath path = new Path(new List<IGraphObject>(walkedPath));
				foundPaths.Add(path);
				return;
			}

			//Bail if current path is longer than shortest found path
			if (foundPaths.Count > 0 && foundPaths.Min(x => x.NodesAndEdges.Count()) < walkedPath.Count)
				return;

			walkedPath.Add(currentNode);

			foreach (var edgeID in currentNode.OutgoingEdgeIds.Concat(currentNode.IncomingEdgeIds))
			{
				var edge = await GetEdge(edgeID);

				//Ignore missing edges
				if (edge == null)
					continue;

				//Ignore self referencing edges
				if (edge.ToNodeId == edge.FromNodeId)
					continue;

				//Ignore previous walked edges
				if (walkedPath.Any(x => x is IEdge && x.Id == edgeID))
					continue;

				//Set next node id based on edge direction
				string nextNodeId;
				if (edge.FromNodeId == currentNode.Id)
					nextNodeId = edge.ToNodeId; //direction forward
				else if (edge.ToNodeId == currentNode.Id)
					nextNodeId = edge.FromNodeId; //dircetion reversed
				else continue;

				//Ignore edges that lead to a previous visited node
				if (walkedPath.Any(x => x is INode && x.Id == nextNodeId))
					continue;

				var node = await GetNode(nextNodeId);

				//Ignore missing node
				if (node == null)
					continue;

				walkedPath.Add(edge);

				await getPath(node, targetNode, foundPaths, walkedPath);

				//Revert step in for other paths
				walkedPath.RemoveAll(x => x is IEdge && x.Id == edge.Id);

				//Revert target node for in case the step in has found a completed path
				walkedPath.RemoveAll(x => x is INode && x.Id == targetNode.Id);
			}

			//Revert step in for other paths
			walkedPath.RemoveAll(x => x is INode && x.Id == currentNode.Id);
		}
	}
}
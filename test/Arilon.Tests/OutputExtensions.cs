using Arilon.Core;
using Arilon.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arilon.Core
{
	public static class OutputExtensions
	{
		public static string DebugOutput(this IEnumerable<IPath> paths)
		{
			return string.Join(" | ", paths.Select(DebugOutput));
		}

		public static string DebugOutput(this IPath path)
		{
			return "Path (" + path.NodesAndEdges.Count() + "): " + path.NodesAndEdges.DebugOutput();
		}

		public static string DebugOutput(this IGraphObject obj)
		{
			if (obj is IEdge edge)
			{
				return $"{edge.Id} ({edge.FromNodeId} -> {edge.ToNodeId})";
			}
			return obj.Id;
		}

		public static string DebugOutput(this IEnumerable<IGraphObject> path)
		{
			return string.Join(" -> ", path.Select(DebugOutput));
		}
	}
}
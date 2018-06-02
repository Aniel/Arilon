using Arilon.Core;
using Arilon.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arilon.Database.InMemory
{
	public class InMemoryDataProvider : BaseDataProvider
	{
		public InMemoryDataProvider(IEnumerable<INode> nodes, IEnumerable<IEdge> edges)
		{
			Nodes = nodes ?? new List<INode>();
			Edges = edges ?? new List<IEdge>();
		}

		public IEnumerable<IEdge> Edges { get; }
		public IEnumerable<INode> Nodes { get; }

		public override Task<IEdge> GetEdge(string id)
		{
			return Task.FromResult(Edges.FirstOrDefault(x => x.Id == id));
		}

		public override Task<INode> GetNode(string id)
		{
			return Task.FromResult(Nodes.FirstOrDefault(x => x.Id == id));
		}
	}
}
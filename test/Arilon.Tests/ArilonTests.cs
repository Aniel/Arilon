using Arilon.Core;
using Arilon.Core.Abstractions;
using Arilon.Database.InMemory;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Arilon.Tests
{
	public class ArilonTests
	{
		[Fact]
		public async Task ShouldFindFather()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", new List<string> { "e1" }, null, new Dictionary<string, string> { { "gender", "female" } });
			var nEnd = new Node("nEnd", null, new List<string> { "e1" }, new Dictionary<string, string> { { "gender", "male" } });
			var e1 = new Edge("e1", "nStart", "nEnd", new Dictionary<string, string> { { "label", "parent" } });
			var nodes = new INode[] { nStart, nEnd };
			var edges = new IEdge[] { e1 };
			var expectedResult = new IPath[] { new Path(new List<IGraphObject> { nStart, e1, nEnd }) };

			var arilon = new Core.Arilon(new InMemoryDataProvider(nodes, edges), new string[] { "label" }, new string[] { "gender" });

			var result = await arilon.CalculateRelationIdentifier("nStart", "nEnd");

			result.StartNode.Id.Should().Be("nStart");
			result.EndNode.Id.Should().Be("nEnd");

			result.Identifier.Should().Be("n(gender:female)->e(label:parent)->n(gender:male)");

			result.Paths.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact]
		public async Task ShouldFindHalfBrother()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", null, new List<string> { "e2" }, new Dictionary<string, string> { { "gender", "female" } });
			var n1 = new Node("n1", new List<string> { "e1", "e2" }, null, new Dictionary<string, string> { { "gender", "unicorn" } });
			var nEnd = new Node("nEnd", null, new List<string> { "e1" }, new Dictionary<string, string> { { "gender", "male" } });
			var e1 = new Edge("e1", "nEnd", "n1", new Dictionary<string, string> { { "label", "parent" } });
			var e2 = new Edge("e2", "nStart", "n1", new Dictionary<string, string> { { "label", "parent" } });
			var nodes = new INode[] { nStart, nEnd, n1 };
			var edges = new IEdge[] { e1, e2 };
			var expectedResult = new IPath[] { new Path(new List<IGraphObject> { nStart, e2, n1, e1, nEnd }) };

			var arilon = new Core.Arilon(new InMemoryDataProvider(nodes, edges), new string[] { "label" }, new string[] { "gender" });

			var result = await arilon.CalculateRelationIdentifier("nStart", "nEnd");

			result.StartNode.Id.Should().Be("nStart");
			result.EndNode.Id.Should().Be("nEnd");

			result.Identifier.Should().Be("n(gender:female)->e(label:parent)->n(gender:unicorn)<-e(label:parent)<-n(gender:male)");

			result.Paths.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact]
		public async Task ShouldFindMother()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", new List<string> { "e1" }, null, new Dictionary<string, string> { { "gender", "male" } });
			var nEnd = new Node("nEnd", null, new List<string> { "e1" }, new Dictionary<string, string> { { "gender", "female" } });
			var e1 = new Edge("e1", "nEnd", "nStart", new Dictionary<string, string> { { "label", "parent" } });
			var nodes = new INode[] { nStart, nEnd };
			var edges = new IEdge[] { e1 };
			var expectedResult = new IPath[] { new Path(new List<IGraphObject> { nStart, e1, nEnd }) };

			var arilon = new Core.Arilon(new InMemoryDataProvider(nodes, edges), new string[] { "label" }, new string[] { "gender" });

			var result = await arilon.CalculateRelationIdentifier("nStart", "nEnd");

			result.StartNode.Id.Should().Be("nStart");
			result.EndNode.Id.Should().Be("nEnd");

			result.Identifier.Should().Be("n(gender:male)<-e(label:parent)<-n(gender:female)");

			result.Paths.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact]
		public async Task ShouldFindSister()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", null, new List<string> { "e2", "e3" }, new Dictionary<string, string> { { "gender", "female" } });
			var n1 = new Node("n1", new List<string> { "e1", "e2" }, null, new Dictionary<string, string> { { "gender", "unicorn" } });
			var n2 = new Node("n2", new List<string> { "e1", "e2", "e3", "e4" }, null, new Dictionary<string, string> { { "gender", "unicorn" } });
			var nEnd = new Node("nEnd", null, new List<string> { "e1", "e4" }, new Dictionary<string, string> { { "gender", "female" } });
			var e1 = new Edge("e1", "nEnd", "n1", new Dictionary<string, string> { { "label", "parent" } });
			var e2 = new Edge("e2", "nStart", "n1", new Dictionary<string, string> { { "label", "parent" } });
			var e3 = new Edge("e3", "nStart", "n2", new Dictionary<string, string> { { "label", "parent" } });
			var e4 = new Edge("e4", "nEnd", "n2", new Dictionary<string, string> { { "label", "parent" } });
			var nodes = new INode[] { nStart, nEnd, n1, n2 };
			var edges = new IEdge[] { e1, e2, e3, e4 };
			var expectedResult = new IPath[]
			{
				new Path(new List<IGraphObject> { nStart, e2, n1, e1, nEnd }),
				new Path(new List<IGraphObject> { nStart, e3, n2, e4, nEnd })
			};

			var arilon = new Core.Arilon(new InMemoryDataProvider(nodes, edges), new string[] { "label" }, new string[] { "gender" });

			var result = await arilon.CalculateRelationIdentifier("nStart", "nEnd");

			result.StartNode.Id.Should().Be("nStart");
			result.EndNode.Id.Should().Be("nEnd");

			result.Identifier.Should().Be("n(gender:female)->e(label:parent)->n(gender:unicorn)<-e(label:parent)<-n(gender:female)&n(gender:female)->e(label:parent)->n(gender:unicorn)<-e(label:parent)<-n(gender:female)");

			result.Paths.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact]
		public void ShouldThrowOnMissingEdgeDatabaseProvider()
		{
			var arilon = new Core.Arilon(new InMemoryDataProvider(null, null));
			Func<Task> fn = async () => { await arilon.CalculateRelationIdentifier("nStart", "nEnd"); };
			fn.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public void ShouldThrowOnNullDatabaseProvider()
		{
			Action fn = () => { var arilon = new Core.Arilon(null); };
			fn.Should().Throw<ArgumentNullException>();
		}
	}
}
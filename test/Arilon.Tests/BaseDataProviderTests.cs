using System;
using Xunit;
using Arilon.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit.Abstractions;
using Arilon.Database.InMemory;
using Arilon.Database.InFiles;
using Arilon.Core.Abstractions;
using FluentAssertions;

namespace Arilon.Tests
{
	public class BaseDataProviderTests
	{
		private readonly ITestOutputHelper _output;

		public BaseDataProviderTests(ITestOutputHelper output)
		{
			this._output = output;
		}

		[Fact]
		public static async Task ShouldFindPathWithLongerPaths()
		{
			var nStart = new Node("nStart", incomingEdgeIds: new List<string> { "e1" }, outgoingEdgeIds: new List<string> { "e2" });
			var n1 = new Node("n1", incomingEdgeIds: new List<string> { "e2", "e3" }, outgoingEdgeIds: new List<string> { "e1", "e6" });
			var n2 = new Node("n2", incomingEdgeIds: new List<string> { "e6", "e5" }, outgoingEdgeIds: new List<string> { "e4", "e3" });
			var nEnd = new Node("nEnd", incomingEdgeIds: new List<string> { "e4" }, outgoingEdgeIds: new List<string> { "e5" });
			var e1 = new Edge("e1", "n1", "nStart");
			var e2 = new Edge("e2", "nStart", "n1");
			var e3 = new Edge("e3", "n2", "n1");
			var e4 = new Edge("e4", "n2", "nEnd");
			var e5 = new Edge("e5", "nEnd", "n2");
			var e6 = new Edge("e6", "n1", "n2");

			var inMemoryProvider = new InMemoryDataProvider(new INode[] { nStart, n1, n2, nEnd }, new IEdge[] { e1, e2, e3, e4, e5, e6 });
			var result = await inMemoryProvider.ShortestPaths("n2", "nEnd");

			result
				.Should()
				.BeEquivalentTo(new IPath[]
				{
					new Path(new IGraphObject[] { n2, e4, nEnd }),
					new Path(new IGraphObject[] { n2, e5, nEnd })
				}, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Theory(DisplayName = "Should Find Path")]
		[MemberData(nameof(PathTestData.SimpleConnection), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.ThreeNodeLine), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.FourNodeLine), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.DoubleConnection), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.ShorterPath), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.SelfReferingNodes), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.DoubleConnectionsGraph), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.MiddleDoubleConnectionPath), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.ThreeNodeLoop), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.ThreeNodeReverseLoop), MemberType = typeof(PathTestData))]
		[MemberData(nameof(PathTestData.ParallelConnectionGraph), MemberType = typeof(PathTestData))]
		public async Task ShouldFindPaths(IEnumerable<INode> nodes, IEnumerable<IEdge> edges, IEnumerable<IPath> expectedResult)
		{
			_output.WriteLine($"Excepted ({expectedResult.Count()}):");
			foreach (var path in expectedResult)
			{
				_output.WriteLine(path.DebugOutput());
			}

			InMemoryDataProvider dataProvider = new InMemoryDataProvider(nodes, edges);
			var result = await dataProvider.ShortestPaths("nStart", "nEnd");

			_output.WriteLine($"Result ({result.Count()}): ");
			foreach (var path in result)
			{
				_output.WriteLine(path.DebugOutput());
			}

			result
				.Should()
				.BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact(DisplayName = "Should handle missing edges")]
		public async Task ShouldHandleMissingEdges()
		{
			var nStart = new Node("nStart", new List<string> { "e2" }, new List<string> { "e1" });
			var nEnd = new Node("nEnd", new List<string> { "e1" }, new List<string> { "e2" });
			var e1 = new Edge("e1", "nStart", "nEnd");
			var e2 = new Edge("e2", "nEnd", "nStart");
			var inMemoryProvider = new InMemoryDataProvider(new INode[] { nStart, nEnd }, new IEdge[] { e1 });
			var result = await inMemoryProvider.ShortestPaths("nStart", "nEnd");

			result
				.Should()
				.BeEquivalentTo(new IPath[]
				{
					new Path(new IGraphObject[] { nStart, e1, nEnd })
				}, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact(DisplayName = "Should handle missing nodes")]
		public async Task ShouldHandleMissingNodes()
		{
			var nStart = new Node("nStart", incomingEdgeIds: new List<string> { "e2" }, outgoingEdgeIds: new List<string> { "e1" });
			var n1 = new Node("n1", incomingEdgeIds: new List<string> { "e6" }, outgoingEdgeIds: new List<string> { "e2", "e4", "e7" });
			var n2 = new Node("n2", incomingEdgeIds: new List<string> { "e1", "e3", "e7" }, outgoingEdgeIds: new List<string> { "e6" });
			var n3 = new Node("n3", incomingEdgeIds: new List<string> { "e4" }, outgoingEdgeIds: new List<string> { "e3", "e5" });
			var nEnd = new Node("nEnd", incomingEdgeIds: new List<string> { "e5" }, outgoingEdgeIds: new List<string> { });
			var e1 = new Edge("e1", "nStart", "n2");
			var e2 = new Edge("e2", "n1", "nStart");
			var e3 = new Edge("e3", "n3", "n2");
			var e4 = new Edge("e4", "n1", "n3");
			var e5 = new Edge("e5", "n3", "nEnd");
			var e6 = new Edge("e6", "n2", "n1");
			var e7 = new Edge("e7", "n1", "n2");

			var inMemoryProvider = new InMemoryDataProvider(
				new INode[] { nStart, n2, n3, nEnd },
				new IEdge[] { e1, e2, e3, e4, e5, e6, e7 });

			var result = await inMemoryProvider.ShortestPaths("nStart", "nEnd");

			result
				.Should()
				.BeEquivalentTo(new IPath[]
				{
					new Path(new IGraphObject[] { nStart, e1, n2, e3, n3, e5, nEnd })
				}, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact(DisplayName = "Should handle no connection")]
		public async Task ShouldHandleNoConenction()
		{
			var n1 = new Node("nStart", new List<string> { "e1" });
			var n2 = new Node("nEnd");
			var e1 = new Edge("e1", "nStart", "nStart");
			var expectedResult = new IPath[0];

			InMemoryDataProvider dataProvider = new InMemoryDataProvider(new List<INode> { n1, n2 }, new List<IEdge> { e1 });
			var result = await dataProvider.ShortestPaths("nStart", "nEnd");

			result
				.Should()
				.BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.NodesAndEdges));
		}

		[Fact(DisplayName = "Should throw on missing end node")]
		public void ShouldThrowOnMissingEndNode()
		{
			InMemoryDataProvider dataProvider = new InMemoryDataProvider(new List<INode> { new Node("nStart") }, new List<IEdge>());
			Func<Task> func = async () => await dataProvider.ShortestPaths("nStart", "nEnd");

			func
				.Should()
				.Throw<ArgumentNullException>();
		}

		[Fact(DisplayName = "Should throw on missing start node")]
		public void ShouldThrowOnMissingStartNode()
		{
			InMemoryDataProvider dataProvider = new InMemoryDataProvider(new List<INode> { new Node("nEnd") }, new List<IEdge>());
			Func<Task> func = async () => await dataProvider.ShortestPaths("nStart", "nEnd");

			func
				.Should()
				.Throw<ArgumentNullException>();
		}
	}
}
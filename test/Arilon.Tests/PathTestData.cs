using Arilon.Core;
using Arilon.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Arilon.Tests
{
	public static class PathTestData
	{
		public static TheoryData<INode[], IEdge[], IPath[]> SimpleConnection()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var n1 = new Node("nStart", new List<string> { "e1" });
			var n2 = new Node("nEnd", null, new List<string> { "e1" });
			var e1 = new Edge("e1", "nEnd", "nStart");
			data.Add(
				new INode[] { n1, n2 },
				new IEdge[] { e1 },
				new IPath[] { new Path(new List<IGraphObject> { n1, e1, n2 }) });
			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> DoubleConnection()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var n1 = new Node("nStart", new List<string> { "e2" }, new List<string> { "e1" });
			var n2 = new Node("nEnd", new List<string> { "e1" }, new List<string> { "e2" });
			var e1 = new Edge("e1", "nStart", "nEnd");
			var e2 = new Edge("e2", "nEnd", "nStart");
			data.Add(
				new INode[] { n1, n2 },
				new IEdge[] { e1, e2 },
				new IPath[] {
					new Path(new IGraphObject[] { n1, e1, n2 }),
					new Path(new IGraphObject[] { n1, e2, n2 })
				});
			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> FourNodeLine()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", null, new List<string> { "e1" });
			var n2 = new Node("n2", new List<string> { "e1" }, new List<string> { "e2" });
			var n3 = new Node("n3", new List<string> { "e2" }, new List<string> { "e3" });
			var nEnd = new Node("nEnd", new List<string> { "e3" });

			var e1 = new Edge("e1", "nStart", "n2");
			var e2 = new Edge("e2", "n2", "n3");
			var e3 = new Edge("e3", "n3", "nEnd");
			data.Add(
				new INode[] { nStart, n2, n3, nEnd },
				new IEdge[] { e1, e2, e3 },
				new IPath[] { new Path(new IGraphObject[] { nStart, e1, n2, e2, n3, e3, nEnd }) });
			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> ThreeNodeLine()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", null, new List<string> { "e1" });
			var n2 = new Node("n2", new List<string> { "e1" }, new List<string> { "e2" });
			var nEnd = new Node("nEnd", new List<string> { "e2" });

			var e1 = new Edge("e1", "nStart", "n2");
			var e2 = new Edge("e2", "n2", "nEnd");
			data.Add(
				new INode[] { nStart, n2, nEnd },
				new IEdge[] { e1, e2 },
				new IPath[] { new Path(new IGraphObject[] { nStart, e1, n2, e2, nEnd }) });
			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> ShorterPath()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();
			var nStart = new Node("nStart", null, new List<string> { "e1", "e3" });
			var n2 = new Node("n2", new List<string> { "e1" }, new List<string> { "e2" });
			var nEnd = new Node("nEnd", new List<string> { "e2", "e3" });

			var e1 = new Edge("e1", "nStart", "n2");
			var e2 = new Edge("e2", "n2", "nEnd");
			var e3 = new Edge("e3", "nStart", "nEnd");
			data.Add(
				new INode[] { nStart, n2, nEnd },
				new IEdge[] { e1, e2, e3 },
				new IPath[] { new Path(new IGraphObject[] { nStart, e3, nEnd }) });
			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> SelfReferingNodes()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

			var nStart = new Node("nStart", new List<string> { "e1" }, new List<string> { "e1", "e2" });
			var n1 = new Node("n1", new List<string> { "e2", "e3" }, new List<string> { "e3", "e4" });
			var nEnd = new Node("nEnd", new List<string> { "e4" });
			var e1 = new Edge("e1", "nStart", "nStart");
			var e2 = new Edge("e2", "nStart", "n1");
			var e3 = new Edge("e3", "n1", "n1");
			var e4 = new Edge("e4", "n1", "nEnd");

			data.Add(
				new INode[] { nStart, n1, nEnd },
				new IEdge[] { e1, e2, e3, e4 },
				new IPath[] { new Path(new IGraphObject[] { nStart, e2, n1, e4, nEnd }) }
				);

			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> DoubleConnectionsGraph()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

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


			data.Add(
				new INode[] { nStart, n1, n2, nEnd },
				new IEdge[] { e1, e2, e3, e4, e5, e6 },
				new IPath[] {
					new Path(new IGraphObject[] { nStart, e2, n1, e6, n2, e4, nEnd }),
					new Path(new IGraphObject[] { nStart, e2, n1, e6, n2, e5, nEnd }),
					new Path(new IGraphObject[] { nStart, e2, n1, e3, n2, e4, nEnd }),
					new Path(new IGraphObject[] { nStart, e2, n1, e3, n2, e5, nEnd }),
					new Path(new IGraphObject[] { nStart, e1, n1, e6, n2, e4, nEnd }),
					new Path(new IGraphObject[] { nStart, e1, n1, e6, n2, e5, nEnd }),
					new Path(new IGraphObject[] { nStart, e1, n1, e3, n2, e4, nEnd }),
					new Path(new IGraphObject[] { nStart, e1, n1, e3, n2, e5, nEnd }),
				});

			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> MiddleDoubleConnectionPath()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

			var nStart = new Node("nStart", incomingEdgeIds: new List<string> { }, outgoingEdgeIds: new List<string> { "e1" });
			var n1 = new Node("n1", incomingEdgeIds: new List<string> { "e1", "e3" }, outgoingEdgeIds: new List<string> { "e2" });
			var n2 = new Node("n2", incomingEdgeIds: new List<string> { "e2" }, outgoingEdgeIds: new List<string> { "e4", "e3" });
			var nEnd = new Node("nEnd", incomingEdgeIds: new List<string> { "e4" }, outgoingEdgeIds: new List<string> { });
			var e1 = new Edge("e1", "nStart", "n1");
			var e2 = new Edge("e2", "n1", "n2");
			var e3 = new Edge("e3", "n2", "n1");
			var e4 = new Edge("e4", "n2", "nEnd");


			data.Add(
				new INode[] { nStart, n1, n2, nEnd },
				new IEdge[] { e1, e2, e3, e4 },
				new IPath[] {
					new Path(new IGraphObject[] { nStart, e1, n1, e2, n2, e4, nEnd }),
					new Path(new IGraphObject[] { nStart, e1, n1, e3, n2, e4, nEnd }),
				});

			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> ThreeNodeLoop()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

			var nStart = new Node("nStart", incomingEdgeIds: new List<string> { "e1" }, outgoingEdgeIds: new List<string> { "e3" });
			var n1 = new Node("n1", incomingEdgeIds: new List<string> { "e2" }, outgoingEdgeIds: new List<string> { "e1" });
			var nEnd = new Node("nEnd", incomingEdgeIds: new List<string> { "e3" }, outgoingEdgeIds: new List<string> { "e2" });
			var e1 = new Edge("e1", "n1", "nStart");
			var e2 = new Edge("e2", "nEnd", "n1");
			var e3 = new Edge("e3", "nStart", "nEnd");


			data.Add(
				new INode[] { nStart, n1, nEnd },
				new IEdge[] { e1, e2, e3 },
				new IPath[] {
					new Path(new IGraphObject[] { nStart, e3, nEnd }),
				});

			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> ThreeNodeReverseLoop()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

			var nStart = new Node("nStart", incomingEdgeIds: new List<string> { "e1" }, outgoingEdgeIds: new List<string> { "e3" });
			var n1 = new Node("n1", incomingEdgeIds: new List<string> { "e3" }, outgoingEdgeIds: new List<string> { "e2" });
			var nEnd = new Node("nEnd", incomingEdgeIds: new List<string> { "e2" }, outgoingEdgeIds: new List<string> { "e1" });
			var e1 = new Edge("e1", "nEnd", "nStart");
			var e2 = new Edge("e2", "n1", "nEnd");
			var e3 = new Edge("e3", "nStart", "n1");


			data.Add(
				new INode[] { nStart, n1, nEnd },
				new IEdge[] { e1, e2, e3 },
				new IPath[] { new Path(new IGraphObject[] { nStart, e1, nEnd }), });

			return data;
		}

		public static TheoryData<INode[], IEdge[], IPath[]> ParallelConnectionGraph()
		{
			var data = new TheoryData<INode[], IEdge[], IPath[]>();

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


			data.Add(
				new INode[] { nStart, n1, n2, n3, nEnd },
				new IEdge[] { e1, e2, e3, e4, e5, e6, e7 },
				new IPath[] {
					new Path(new IGraphObject[] { nStart, e1, n2, e3, n3, e5, nEnd }),
					new Path(new IGraphObject[] { nStart, e2, n1, e4, n3, e5, nEnd }),

				});

			return data;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;
using Arilon.Core.Abstractions;

namespace Arilon.Core
{
	/// <summary>
	/// Represents a result of an <seealso cref="IArilon.CalculateRelationIdentifier(INode, INode)"/>
	/// or <seealso cref="IArilon.CalculateRelationIdentifier(string, string)"/> calculation
	/// </summary>
	public class ArilonResult
	{
		/// <summary>
		/// Creates an instance of <see cref="ArilonResult"/>
		/// </summary>
		/// <param name="identifier"> The <see cref="Identifier"/> that describes the relationship</param>
		/// <param name="startNode">  The <see cref="StartNode"/> of the relationship</param>
		/// <param name="endNode">    The <see cref="EndNode"/> of the relationship</param>
		/// <param name="paths">
		/// The shortest <see cref="Paths"/> Paths between the <see cref="StartNode"/> and <see cref="EndNode"/>
		/// </param>
		public ArilonResult(string identifier, INode startNode, INode endNode, IEnumerable<ArilonResultPath> paths)
		{
			Paths = paths;
			Identifier = identifier;
			StartNode = startNode;
			EndNode = endNode;
		}

		/// <summary>
		/// The shortest paths between the <see cref="StartNode"/> and <see cref="EndNode"/>
		/// </summary>
		public IEnumerable<ArilonResultPath> Paths { get; protected internal set; }

		/// <summary>
		/// The end node of the relationship
		/// </summary>
		public INode EndNode { get; set; }

		/// <summary>
		/// The identifier that describes the relationship between the <see cref="StartNode"/> and
		/// <see cref="EndNode"/>
		/// </summary>
		public string Identifier { get; protected internal set; }

		/// <summary>
		/// The start node of the relationship
		/// </summary>
		public INode StartNode { get; set; }
	}
}
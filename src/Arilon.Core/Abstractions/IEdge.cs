namespace Arilon.Core.Abstractions
{
	/// <summary>
	/// Represents an edge in a graph
	/// </summary>
	public interface IEdge : IGraphObject
	{
		/// <summary>
		/// The id of the node at the start of the edge
		/// </summary>
		string FromNodeId { get; }

		/// <summary>
		/// The id of the node at the end of the edge
		/// </summary>
		string ToNodeId { get; }
	}
}
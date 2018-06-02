using System;
using System.Collections.Generic;
using System.Text;

namespace Arilon.Core.Abstractions
{
	/// <summary>
	/// Represents a object in a graph
	/// </summary>
	public interface IGraphObject
	{
		/// <summary>
		/// The id of the object
		/// </summary>
		string Id { get; }

		/// <summary>
		/// The properties of the object
		/// </summary>
		Dictionary<string, string> Properties { get; }
	}
}
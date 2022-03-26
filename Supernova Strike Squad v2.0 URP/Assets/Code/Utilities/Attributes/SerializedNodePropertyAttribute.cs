using System;

namespace Supernova.Utilities.Attributes {
	
	/// <summary>
	/// Used in View node graph system to mark a property a settable in the graph through reflection
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class SerializedNodePropertyAttribute : Attribute { }
}
using System;

namespace Supernova.Utilities.Attributes {
	
	/// <summary>
	/// Mark an attribute as a view output node for the View node graph
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ViewOutputAttribute : Attribute { }
}
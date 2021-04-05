using System.Collections.Generic;
using Mirror;

public class NodeData
{
	public string NodeName;
	public string NodeDescription;

	public int NodeDepth;
	public int NodeIndex;

	public List<int> Connections;

	[System.NonSerialized]
	public NodeEvent Event;

	public NodeData()
	{
	}
}

using System.Collections.Generic;

public class NodeData
{
	public string Name = "";
	public string Description = "";

	public int Depth;

	public NodeEvent Event;

	public List<int> ConnectedNodes = new List<int>();
}

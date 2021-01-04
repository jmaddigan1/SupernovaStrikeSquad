using System.Collections.Generic;

public class NodeData
{
	public string Name = "";
	public string Description = "";

	public int Depth;

	// TODO: This is hard coded so ALL NODES are arena events
	// This should be fixed later
	public NodeEvent Event = new NodeEvent_Arean();

	public List<int> ConnectedNodes = new List<int>();
}

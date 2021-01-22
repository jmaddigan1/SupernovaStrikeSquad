using System.Collections.Generic;

[System.Serializable]
public class NodeData
{
	public string Name = "";
	public string Description = "";

	public int Depth;
	public int Index;

	// The Event is specific to each node event
	// For example this could be am Arena event or a Interaction event

	// When the server sends a NodeMap to a client
	// The  Event is lost
	public NodeEvent Event = new NodeEvent_Arean();

	// NodeEventData is a general struct we use to create a node event when a client receives the data from the server
	// NOTE: This holes everything that is needed for EVERY event type
	// public NodeEventData EventData;

	public List<int> ConnectedNodes = new List<int>();
}

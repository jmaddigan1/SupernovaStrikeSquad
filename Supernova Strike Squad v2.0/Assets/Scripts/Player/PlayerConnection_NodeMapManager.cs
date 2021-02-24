using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// The Player Connection - NodeMap Manager
// The NodeMap Manager Receives and Broadcasts commands related to the NodeMap
public class PlayerConnection_NodeMapManager : NetworkBehaviour
{
	// If we are testing the game we can go to ANY node regardless of our current depth in the NodeMap
	public bool ConstrainedToDepth = false;

	// The server has sent this client new NodeMap Data as a Json string
	// We want to send it to the NodeMap Menu 
	[ClientRpc]
	public void RpcBroadcastNewNodeMap(string mapJson) 
		=> NodeMapMenu.Instance.GenerateNodeMap(mapJson);

	// A Client has clicked on a Node they want to start
	// We send the index of that node to the server and try to start a new event
	[Command]
	public void CmdStartNewNode(int index)
	{
		// We get the current NodeMap Data from the SERVERS version of the current game
		NodeMapData nodemap = NodeMapMenu.Instance.CurrentNodeMap_Server;

		// We find the node the client has to us they want to start
		NodeData selectedNode = nodemap.Nodes[index];

		if (selectedNode == null)
		{
			Debug.LogError($"ERROR: NodeID| {index} does not exist on the server!");
		}
		else
		{
			// Check if the new node is the correct depth OR if we don't need to 'ConstrainedToDepth'
			if (!ConstrainedToDepth || selectedNode.Depth == nodemap.CurrentDepth)
			{
				NodeMapMenu.Instance.StartEvent(NodeMapMenu.Instance.CurrentNodeMap_Server.Nodes[index].Event);
			}
		}	
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection_NodeMapManager : NetworkBehaviour
{
	[ClientRpc]
	public void RpcBroadcastNewNodeMap(string mapJson)
	{
		if (NodeMapMenu.Instance == null)
		{
			Debug.Log("There is no NodeMapMenu.Instance");
			return;
		}

		NodeMapMenu.Instance.GenerateNodeMap(mapJson);
	}

	[Command]
	public void CmdStartNewNode(int index)
	{
		NodeMapData nodemap = NodeMapMenu.Instance.CurrentNodeMap_Server;

		NodeData selectedNode = nodemap.Nodes[index];


		// (TEMP) TODO: Remove this
		bool constrainedToDepth = false;


		if (selectedNode == null)
		{
			Debug.LogError($"ERROR: NodeID: {index} does not exist!");
			return;
		}

		if (!constrainedToDepth || selectedNode.Depth == nodemap.CurrentDepth)
		{
			NodeMapMenu.Instance.StartEvent(NodeMapMenu.Instance.CurrentNodeMap_Server.Nodes[index].Event);
		}
		else
		{
			Debug.LogError($"ERROR: NodeID: {index} is not a valid node!");
		}
	}
}

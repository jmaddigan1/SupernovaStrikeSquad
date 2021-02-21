using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection_NodeMapManager : NetworkBehaviour
{
	[ClientRpc]
	public void RpcBroadcastNewNodeMap(string mapJson)
	{
		Debug.Log(mapJson);

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
		NodeMapMenu.Instance.StartEvent(NodeMapMenu.Instance.CurrentNodeMap_Server.Nodes[index].Event);
	}
}

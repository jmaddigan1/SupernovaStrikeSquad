using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NodeMap : NetworkBehaviour
{
	[SerializeField] private NodeMapEventManager eventManager = null;

	[SerializeField] private DepthGroup depthGroupPrefab = null;
	[SerializeField] private Node nodePrefab = null;

	[SerializeField] private Menu menuBehaviour = null;

	public Transform ContentAnchor = null;

	public NodeMapData CurrentNodemap;
	public NodeData CurrentNode;

	public List<Node> Nodes = new List<Node>();

	public void StartNodeMap(GameManagerSettings settings)
	{
		eventManager = GetComponent<NodeMapEventManager>();

		ParseNodeMapInitializationData(settings);

		Rpc_PausePlayer(true);

		Rpc_BuildMap(CurrentNodemap, CurrentNode);
	}

	void ParseNodeMapInitializationData(GameManagerSettings settings)
	{
		// DEFAULT TEST
		if (settings.MissionTypes == MissionTypes.None)
		{

		}

		// 
		if (settings.MissionTypes == MissionTypes.Campaign) { }
		if (settings.MissionTypes == MissionTypes.MissionBoard) { }
		if (settings.MissionTypes == MissionTypes.Endless) { }

		// TMP
		CurrentNodemap = NodeMapPresets.TestMap();
		CurrentNode = CurrentNodemap.Nodes[0];
	}

	[ClientRpc]
	public void Rpc_BuildMap(NodeMapData newMapData, NodeData newNodeData)
	{
		menuBehaviour.OpenMenu(null, false);

		ClearNodeMap();

		// Only update the current Node Map and Node if we are NOT the servers
		if (!isServer )
		{
			CurrentNodemap = newMapData;
			CurrentNode = newNodeData;
		}

		// DEPTH
		for (int depth = 0; depth < newMapData.MapDepth; depth++)
		{
			Instantiate(depthGroupPrefab, ContentAnchor);
		}

		// NODES
		foreach (NodeData nodeData in newMapData.Nodes)
		{
			Node node = Instantiate(nodePrefab, ContentAnchor.GetChild(nodeData.NodeDepth));
			Nodes.Add(node.Init(nodeData, CurrentNode, CurrentNodemap));
		}
	}

	[Server]
	public void SelectNewNode(int nodeIndex)
	{
		if (ValidateNode(nodeIndex))
		{
			Rpc_PausePlayer(false);

			NodeData nodeData = CurrentNodemap.Nodes[nodeIndex];
			NodeEvent nodeEvent = nodeData.Event;

			CurrentNode = nodeData;

			StartNewEvent();
		}
	}

	[Server]
	public void StartNewEvent()
	{
		Rpc_HideNodeMap();
		eventManager.StartNewEvent(CurrentNode.Event);
	}

	[ClientRpc]
	public void Rpc_HideNodeMap()
	{
		menuBehaviour.CloseMenu();
	}

	[Server]
	public void CompleteNode()
	{
		if (CurrentNode.NodeDepth == CurrentNodemap.MapDepth - 1)
		{
			GameManager.Instance.OnMissionComplete(true);
		}
		else
		{
			// Increment the map depth
			CurrentNodemap.MapCurrentDepth = CurrentNode.NodeDepth;

			Rpc_BuildMap(CurrentNodemap, CurrentNode);
		}
	}

	[Server]
	public bool ValidateNode(int nodeIndex)
	{
		// First Node
		if (CurrentNode == null) return (nodeIndex == 0);


		if (nodeIndex >= CurrentNodemap.Nodes.Count) {
			return false;
		}

		NodeData node = CurrentNodemap.Nodes[nodeIndex];

		if (CurrentNode.Connections.Contains(node.NodeIndex))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void ClearNodeMap()
	{
		// NOTE: The reason we do it this way is because for some reason
		// The Destroy method was destroying the NEW Depth grounds that spawn 
		// after we clear the old ones

		Nodes.Clear();

		foreach (Transform child in ContentAnchor)
		{
			// Hide the current gameobject
			child.gameObject.SetActive(false);

			// Destroy it after a short delay
			Destroy(child.gameObject, 0.1f);
		}

		ContentAnchor.DetachChildren();
	}

	[ClientRpc]
	public void Rpc_PausePlayer(bool state)
	{
		ShipController.Interacting = state;
	}
}

public class NodeMapData
{
	public List<NodeData> Nodes;

	public int MapDepth;
	public int MapCurrentDepth;

	public NodeMapData()
	{
	}
}

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NodeMap : NetworkBehaviour
{
	[SerializeField] private NodeMapEventManager eventManager = null;
	[SerializeField] private Transform ContentAnchor = null;
	[SerializeField] private DepthGroup DepthGroupPrefab = null;
	[SerializeField] private Node NodePrefab = null;

	public NodeMapData CurrentNodemap;
	public NodeData CurrentNode;

	public List<Node> Nodes = new List<Node>();

	public override void OnStartServer()
	{
		eventManager = GetComponent<NodeMapEventManager>();
		CurrentNodemap = NodeMapPresets.TestMap();
	}
	public override void OnStartClient()
	{
		DontDestroyOnLoad(transform.parent.gameObject);
	}

	private void Update()
	{
		if (isServer)
		{
			// TEMP
			// Send the current Node Map to all clients
			if (Input.GetKeyDown(KeyCode.P))
			{
				Rpc_BuildMap(CurrentNodemap);
			}
		}
	}

	[ClientRpc]
	public void Rpc_BuildMap(NodeMapData data)
	{
		PausePlayer(true);
		ClearNodeMap();

		ContentAnchor.gameObject.SetActive(true);

		Debug.Log("Node Node Map");
		Debug.Log(data.MapCurrentDepth);

		// DEPTH
		for (int depth = 0; depth < data.MapDepth; depth++)
		{
			Instantiate(DepthGroupPrefab, ContentAnchor);
		}

		// NODES
		foreach (NodeData nodeData in data.Nodes)
		{
			Node node = Instantiate(NodePrefab, ContentAnchor.GetChild(nodeData.NodeDepth));
			Nodes.Add(node.Init(this, nodeData));
		}
	}

	[Server]
	public void SelectNewNode(int nodeIndex)
	{
		if (ValidateNode(nodeIndex))
		{
			PausePlayer(false);

			ContentAnchor.gameObject.SetActive(false);

			NodeData nodeData = CurrentNodemap.Nodes[nodeIndex];
			NodeEvent nodeEvent = nodeData.Event;

			CurrentNode = nodeData;

			StartNewEvent();
		}
	}

	[Server]
	public void StartNewEvent()
	{
		eventManager.StartNewEvent();
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

			Rpc_BuildMap(CurrentNodemap);
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

		if (CurrentNode.Connections.Contains(node.NodeIndex)) {
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

	void PausePlayer(bool state)
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
	public string NodeName = "UnNamed";
	public string NodeDescription = "NoDescription";

	public NodeEvent Event;

	public int NodeDepth;
	public int NodeIndex;

	public List<int> Connections;

	public NodeData()
	{
	}
}
public class NodeEvent
{
	public string EventName;
	public string EventDescription;
	
	public NodeEvent()
	{
	}
}

public static class NodeMapPresets
{
	public static NodeMapData TestMap()
	{
		NodeMapData nodeMap = new NodeMapData()
		{
			MapDepth = 5,

			Nodes = new List<NodeData>()
			{
				MakeNode("Node A", 0, new List<int>(){ 1, 5 },		NodeEventPresets.TestEvent() ),
				MakeNode("Node B", 1, new List<int>(){ 2,3,4 }, NodeEventPresets.TestEvent() ),
				MakeNode("Node C", 2, new List<int>(){ 5 },		NodeEventPresets.TestEvent() ),
				MakeNode("Node D", 2, new List<int>() {5,6 },	NodeEventPresets.TestEvent() ),
				MakeNode("Node E", 2, new List<int>(){ 5,7 },	NodeEventPresets.TestEvent() ),
				MakeNode("Node F", 3, new List<int>(){ 7 },		NodeEventPresets.TestEvent() ),
				MakeNode("Node G", 3, new List<int>(){ 7 },		NodeEventPresets.TestEvent() ),
				MakeNode("Node H", 4, new List<int>(){ },		NodeEventPresets.TestEvent() ),
			}
		};

		// Give each node its index
		for (int index = 0; index < nodeMap.Nodes.Count; index++)
			nodeMap.Nodes[index].NodeIndex = index;

		return nodeMap;
	}

	public static NodeData MakeNode(string name, int depth, List<int> connections = null, NodeEvent nodeEvent = null)
	{
		return new NodeData()
		{
			NodeName = name,
			NodeDepth = depth,

			Connections = connections,

			Event = nodeEvent
		};
	}
}
public static class NodeEventPresets
{
	public static NodeEvent TestEvent()
	{
		NodeEvent nodeEvent = new NodeEvent()
		{
			EventName = "Test Event",

			EventDescription = "A description for the test Node"
		};

		return nodeEvent;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaigns : MonoBehaviour
{
	public static NodeMapData TestCampaign()
	{
		NodeMapData nodeMap =  new NodeMapData
		{
			Name = "Test Campaign",

			Depth = 6,

			Nodes = new List<NodeData> {
				new NodeData { Depth = 0, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ 1, 2 } },

				new NodeData { Depth = 1, Event = Events.LargeArena(), ConnectedNodes = new List<int>{ 3 } },
				new NodeData { Depth = 1, Event = Events.LargeArena(), ConnectedNodes = new List<int>{ 3 } },

				new NodeData { Depth = 2, Event = Events.ClusterArena(), ConnectedNodes = new List<int>{ 4, 7 } },

				new NodeData { Depth = 3, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ 5, 6, 7 } },

				new NodeData { Depth = 4, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Depth = 4, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Depth = 4, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ 8 } },

				new NodeData { Depth = 5, Event = Events.ArenaWaves(), ConnectedNodes = new List<int>{ } },
			}
		};

		return InitializeNodeMap(nodeMap);
	}

	public static NodeMapData InitializeNodeMap( NodeMapData nodeMap)
	{
		foreach (NodeData node in nodeMap.Nodes)
		{
			if (node.Depth > nodeMap.Depth)
			{
				Debug.LogError($"Node Map|{nodeMap.Name}: Has a NODE that has an invalid DEPTH");
			}

			node.Name = node.Event.Name;
		}

		return nodeMap;
	}
}

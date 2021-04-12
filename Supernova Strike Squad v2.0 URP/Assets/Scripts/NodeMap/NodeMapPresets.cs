using System.Collections.Generic;

public static class NodeMapPresets
{
	public static NodeMapData TestMap()
	{
		NodeMapData nodeMap = new NodeMapData()
		{
			MapDepth = 5,

			Nodes = new List<NodeData>()
			{
				MakeNode("Node A", 0, new List<int>() { 1, 5 },      NodeEventPresets.TestAreana() ),
				MakeNode("Node B", 1, new List<int>() { 2,3,4 },     NodeEventPresets.TestBoss() ),
				MakeNode("Node C", 2, new List<int>() { 5 },         NodeEventPresets.TestRunner()   ),
				MakeNode("Node D", 2, new List<int>() { 5,6 },       NodeEventPresets.TestBoss()   ),
				MakeNode("Node E", 2, new List<int>() { 5,7 },       NodeEventPresets.TestAreana() ),
				MakeNode("Node F", 3, new List<int>() { 7 },         NodeEventPresets.TestAreana() ),
				MakeNode("Node G", 3, new List<int>() { 7 },         NodeEventPresets.TestAreana() ),
				MakeNode("Node H", 4, new List<int>() { },           NodeEventPresets.TestAreana() ),
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

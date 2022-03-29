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
				MakeNode( 0, new List<int>() { 1, 2, 5 },     SNSSPresets.TestAreana() ),
				MakeNode( 1, new List<int>() { 2, 3, 4 },     SNSSPresets.TestRunner() ),
				MakeNode( 2, new List<int>() { 5 },           SNSSPresets.TestBoss()   ),
				MakeNode( 2, new List<int>() { 5,6 },         SNSSPresets.TestAreana() ),
				MakeNode( 2, new List<int>() { 5,7 },         SNSSPresets.TestAreana() ),
				MakeNode( 3, new List<int>() { 7 },           SNSSPresets.TestAreana() ),
				MakeNode( 3, new List<int>() { 7 },           SNSSPresets.TestAreana() ),
				MakeNode( 4, new List<int>() { },             SNSSPresets.TestAreana() ),
			}
		};

		// Give each node its index
		for (int index = 0; index < nodeMap.Nodes.Count; index++)
			nodeMap.Nodes[index].NodeIndex = index;

		return nodeMap;
	}

	public static NodeData MakeNode(int depth, List<int> connections = null, NodeEvent nodeEvent = null)
	{
		return new NodeData()
		{
			NodeName = depth + " | " +  nodeEvent.EventName,
			NodeDescription = nodeEvent.EventDescription,

			NodeDepth = depth,

			Connections = connections,

			Event = nodeEvent
		};
	}
}

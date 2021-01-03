using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NodeMapDepthController))]
[RequireComponent(typeof(NodeMapNodeController))]

public class NodeMapMenu : MonoBehaviour
{
	[SerializeField]
	public NodeMapDepthController DepthController;

	[SerializeField]
	public NodeMapNodeController NodeController;

	[SerializeField]
	private Node NodePrafab;


	// Private Members
	private bool eventRunning = false;

	private void Awake()
	{
		NodeMapData mapData = new NodeMapData
		{
			Depth = 6,

			Nodes = new List<NodeData> {
				new NodeData { Name = "TempName 0", Depth = 0, Event = new NodeEvent_Arean(),  ConnectedNodes = new List<int>{ 1, 2 } },

				new NodeData { Name = "TempName 1", Depth = 1, ConnectedNodes = new List<int>{ 3 } },
				new NodeData { Name = "TempName 2", Depth = 1, ConnectedNodes = new List<int>{ 3 } },

				new NodeData { Name = "TempName 3", Depth = 2, ConnectedNodes = new List<int>{ 4, 7 } },

				new NodeData { Name = "TempName 4", Depth = 3, ConnectedNodes = new List<int>{ 5, 6, 7 } },

				new NodeData { Name = "TempName 5", Depth = 4, ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Name = "TempName 6", Depth = 4, ConnectedNodes = new List<int>{ 8 } },
				new NodeData { Name = "TempName 7", Depth = 4, ConnectedNodes = new List<int>{ 8 } },

				new NodeData { Name = "TempName 8", Depth = 5, ConnectedNodes = new List<int>{ } },
			}
		};

		GenerateNodeMap(mapData);
	}

	public void GenerateNodeMap(NodeMapData mapData)
	{
		DepthController.Init(mapData.Depth);

		foreach (NodeData node in mapData.Nodes)
		{
			NodeController.NodeList.Add(Instantiate(NodePrafab, DepthController.GetDepthAnchor(node.Depth)).Init(this, node));
		}
	}

	#region [Region] Run Event Methods

	public IEnumerator StartNewEvent(NodeEvent eventData)
	{
		if (!eventRunning)
		{
			eventRunning = true;

			yield return CloseMenu();

			yield return PlayEvent(eventData);		

			eventRunning = false;

			yield return OpenMenu();
		}
	}

	private IEnumerator PlayEvent(NodeEvent eventData)
	{
		eventData.OnStartEvent();

		while (eventData.IsOver() == false) {
			yield return null;
		}

		eventData.OnEndEvent();
	}

	private IEnumerator OpenMenu()
	{
		yield return null;
	}

	private IEnumerator CloseMenu()
	{
		yield return null;
	}

	#endregion
}

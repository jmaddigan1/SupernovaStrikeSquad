using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
	[SerializeField]
	private Button Button = null;

	public NodeMapMenu NodeMap;
	public NodeData NodeData;

	public Node Init(NodeMapMenu nodeMap, NodeData nodeData)
	{
		NodeMap = nodeMap;
		NodeData = nodeData;

		Button.onClick.AddListener(() => { StartCoroutine(nodeMap.StartNewEvent(NodeData.Event)); });

		return this;
	}

	void OnDrawGizmos()
	{
		foreach (Node node in NodeMap.NodeController.GetNodes(NodeData.ConnectedNodes))
		{
			Gizmos.DrawLine(transform.position, node.transform.position);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
	[SerializeField]
	private Button Button = null;

	[SerializeField]
	private Text NodeName = null;

	public NodeMapMenu NodeMap;
	public NodeData NodeData;

	public Node Init(NodeMapMenu nodeMap, NodeData nodeData)
	{
		NodeMap = nodeMap;
		NodeData = nodeData;

		NodeName.text = nodeData.Name;

		Button.onClick.AddListener(() => {
			PlayerConnection.LocalPlayer.Map.CmdStartNewNode(NodeData.Index);
		});

		return this;
	}

	private void Update()
	{
		if (TryGetComponent<Image>(out Image image))
		{
			if (NodeData.Depth < NodeMap.CurrentNodeMap_Client.CurrentDepth) {
				image.color = Color.red;
			}
			else if (NodeData.Depth > NodeMap.CurrentNodeMap_Client.CurrentDepth) {
				image.color = Color.blue;
			}
			else {
				image.color = Color.green;
			}
		}
	}

	void OnDrawGizmos()
	{
		foreach (Node node in NodeMap.NodeController.GetNodes(NodeData.ConnectedNodes))
		{
			Gizmos.DrawLine(transform.position, node.transform.position);
		}
	}
}

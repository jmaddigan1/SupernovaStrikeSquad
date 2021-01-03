using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMapNodeController : MonoBehaviour
{
	public List<Node> NodeList = new List<Node>();

	public List<Node> GetNodes(List<int> nodesToGet)
	{
		List<Node> nodes = new List<Node>();

		foreach (int nodeIndex in nodesToGet)
		{
			nodes.Add(NodeList[nodeIndex]);
		}

		return nodes;
	}
}

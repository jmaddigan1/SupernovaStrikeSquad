using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private TextMeshProUGUI nodeName = null;
	[SerializeField] private Image nodeSprite = null;

	public NodeData Data;
	public NodeMap Map;

	public Node Init(NodeMap nodemap, NodeData nodeData)
	{
		Data = nodeData;
		Map = nodemap;

		nodeName.text = Data.NodeName;

		UpdateColor();

		return this;
	}

	public void UpdateColor()
	{
		#region TODO: Make this better

		if (Map == null) return;

		if (Map.CurrentNode == null)
		{
			if (Data.NodeIndex == 0)
			{
				nodeSprite.color = Color.green;
				return;
			}
			else
			{
				nodeSprite.color = Color.red;
				return;
			}
		}
		else
		{
			if (Data.NodeIndex == Map.CurrentNode.NodeIndex)
			{
				nodeSprite.color = Color.blue;
				return;
			}

			if (Map.CurrentNode.Connections.Contains(Data.NodeIndex))
			{
				nodeSprite.color = Color.green;
				return;
			}

			if ( Map.CurrentNode.NodeIndex > Data.NodeIndex)
			{
				nodeSprite.color = Color.blue + Color.white;
				return;
			}

			if (Map.CurrentNode.NodeIndex < Data.NodeIndex)
			{
				nodeSprite.color = Color.red ;
				return;
			}
		}
		#endregion
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Player.LocalPlayer.Self.Cmd_ClickNode(Data.NodeIndex);
	}

	private void OnDrawGizmos()
	{
		if (Map == null) return;

		foreach (int nodeIndex in Data.Connections)
		{
			Gizmos.DrawLine(transform.position, Map.Nodes[nodeIndex].transform.position);
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private TextMeshProUGUI nodeName = null;
	[SerializeField] private Image nodeSprite = null;

	public NodeData Data;

	public Node Init(NodeData myNodeData, NodeData currentNodeData, NodeMapData currentNodeMapData)
	{
		Data = myNodeData;

		nodeName.text = Data.NodeName;

		UpdateColor(currentNodeData, currentNodeMapData);

		return this;
	}

	public void UpdateColor(NodeData currentNodeData, NodeMapData currentNodeMapData)
	{
		#region TODO: Make this better

		if (currentNodeMapData == null) return;

		if (currentNodeData == null)
		{
			if (Data.NodeIndex == 0)
			{
				nodeSprite.color = Color.green ;
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
			if (Data.NodeIndex == currentNodeData.NodeIndex)
			{
				nodeSprite.color = Color.blue;
				return;
			}

			if (currentNodeData.Connections.Contains(Data.NodeIndex))
			{
				nodeSprite.color = Color.green;
				return;
			}

			if (currentNodeData.NodeIndex > Data.NodeIndex)
			{
				nodeSprite.color = Color.blue + Color.white;
				return;
			}

			if (currentNodeData.NodeIndex < Data.NodeIndex)
			{
				nodeSprite.color = Color.red;
				return;
			}
		}
		#endregion
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Player.LocalPlayer.Self.Cmd_ClickNode(Data.NodeIndex);
	}

	//private void OnDrawGizmos()
	//{
	//	if (Map == null) return;

	//	foreach (int nodeIndex in Data.Connections)
	//	{
	//		Gizmos.DrawLine(transform.position, Map.Nodes[nodeIndex].transform.position);
	//	}
	//}
}

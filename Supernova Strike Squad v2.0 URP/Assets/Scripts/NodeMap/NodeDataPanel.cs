using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeDataPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText = null;
	[SerializeField] private TextMeshProUGUI DescriptionText = null;

	public void UpdateData(NodeData data)
	{
		nameText.text = data.NodeName;
		DescriptionText.text = data.NodeDescription;
	}
}

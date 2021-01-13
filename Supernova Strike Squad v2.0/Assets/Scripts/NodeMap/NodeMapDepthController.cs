using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMapDepthController : MonoBehaviour
{
	[SerializeField]
	private Transform DepthPrefab = null;

	[SerializeField]
	private Transform DepthAnchor = null;

	public List<Transform> DepthList = new List<Transform>();

	public void Init(int depthLayers)
	{
		for (int index = 0; index < depthLayers; index++)
		{
			DepthList.Add(Instantiate(DepthPrefab, DepthAnchor));
		}
	}

	public Transform GetDepthAnchor(int depth) => DepthList[depth];
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NodeMapEventManager : NetworkBehaviour
{
	[SerializeField] private Menu menuBehaviour = null;

	private NodeMap nodeMap;

	public override void OnStartServer()
	{
		nodeMap = GetComponent<NodeMap>();
	}

	[Server] public void StartNewEvent()
		=> StartCoroutine(coStartNewEvent());

	[Server] public void EndEvent()
		=> StartCoroutine(coEndEvent());

	IEnumerator coStartNewEvent()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		GameManager.Instance.Rpc_FadeOutLoadingScreen(true);
		//nodeMap.ContentAnchor.gameObject.SetActive(false);

		yield return new WaitForSecondsRealtime(1.5f);

		// TEMP
		EndEvent();
	}

	IEnumerator coEndEvent()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		GameManager.Instance.Rpc_FadeInLoadingScreen(true);
		//nodeMap.ContentAnchor.gameObject.SetActive(true);

		yield return new WaitForSecondsRealtime(0.5f);

		// TEMP
		nodeMap.CompleteNode();
	}
}

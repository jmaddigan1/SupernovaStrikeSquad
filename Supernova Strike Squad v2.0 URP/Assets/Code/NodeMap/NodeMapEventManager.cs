using System;
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

	[Server]
	public void StartNewEvent(NodeEvent nodeEvent)
		=> StartCoroutine(coStartNewEvent(nodeEvent));

	[Server]
	public void EndEvent()
		=> StartCoroutine(coEndEvent());

	//
	IEnumerator coStartNewEvent(NodeEvent nodeEvent)
	{
		yield return coPlayEvent(nodeEvent);
	}

	IEnumerator coPlayEvent(NodeEvent nodeEvent)
	{
		nodeEvent.OnEventStart();

		GameManager.Instance.Rpc_FadeOutLoadingScreen(true);

		yield return new WaitForSecondsRealtime(1.5f);

		nodeMap.Rpc_PausePlayer(false);

		yield return new WaitForSecondsRealtime(2.5f);

		while (!nodeEvent.IsEventOver())
		{
			yield return null;
		}

		GameManager.Instance.Rpc_FadeInLoadingScreen(true);

		yield return new WaitForSecondsRealtime(0.5f);

		nodeMap.Rpc_PausePlayer(true);

		nodeEvent.OnEventEnd();

		yield return new WaitForSecondsRealtime(1.5f);

		EndEvent();
	}

	IEnumerator coEndEvent()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		nodeMap.CompleteNode();
	}
}
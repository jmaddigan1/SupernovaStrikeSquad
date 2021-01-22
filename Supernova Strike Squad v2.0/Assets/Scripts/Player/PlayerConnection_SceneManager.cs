using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerConnection_SceneManager : NetworkBehaviour
{
	public GameObject NodeMapPrefab = null;

	[ClientRpc]
	public void RpcLoadGameScene(GameData data) => PlayerConnection.LocalPlayer.StartCoroutine(coLoadGameScene());

	[ClientRpc]
	public void RpcLoadHangarScene() => PlayerConnection.LocalPlayer.StartCoroutine(coLoadHangarScene());

	public IEnumerator coLoadGameScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

		while (!asyncLoad.isDone) yield return null;

		yield return new WaitForSecondsRealtime(0.25f);

		PlayerConnection.LocalPlayer.PlayerSceneManager.CmdSpawnNodemap();
	}

	public IEnumerator coLoadHangarScene()
	{
		yield return null;
	}

	[Command]
	public void CmdSpawnNodemap()
	{
		NetworkServer.Spawn(Instantiate(NodeMapPrefab));
	}
}

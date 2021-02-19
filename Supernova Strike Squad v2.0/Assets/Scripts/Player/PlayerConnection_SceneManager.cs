using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerConnection_SceneManager : NetworkBehaviour
{
	public GameObject NodeMapPrefab = null;

	[ClientRpc]
	public void RpcLoadGameScene() => 
		PlayerConnection.LocalPlayer.StartCoroutine(coLoadGameScene());

	[ClientRpc]
	public void RpcLoadHangarScene(bool online) => 
		PlayerConnection.LocalPlayer.StartCoroutine(coLoadHangarScene(online));

	IEnumerator coLoadGameScene()
	{
		// Load and wait for the scene to be loaded
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
		while (!asyncLoad.isDone) yield return null;

		yield return new WaitForSecondsRealtime(0.25f);

		if (isServer && NodeMapMenu.Instance == null) NetworkServer.Spawn(Instantiate(NodeMapPrefab));

		PlayerConnection.LocalPlayer.Object.CmdSpawnShipIntoGames();
	}

	IEnumerator coLoadHangarScene(bool online)
	{
		string scene = online ? "Hangar_Steam" : "Hangar_Local";

		// Load and wait for the scene to be loaded
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while (!asyncLoad.isDone) yield return null;

		yield return new WaitForSecondsRealtime(0.25f);

		if (isServer && NodeMapMenu.Instance) NetworkServer.Destroy(NodeMapMenu.Instance.gameObject);

		PlayerConnection.LocalPlayer.Object.CmdSpawnCharacterIntoGames();
	}
}

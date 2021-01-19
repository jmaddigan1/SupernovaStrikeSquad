using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerConnection_SceneManager : NetworkBehaviour
{
	//[ClientRpc]
	//public void RpcLoadGameScene(GameData data) => PlayerConnection.LocalPlayer.StartCoroutine(coLoadGameScene());

	//[ClientRpc]
	//public void RpcLoadHangarScene() => PlayerConnection.LocalPlayer.StartCoroutine(coLoadHangarScene());

	//public IEnumerator coLoadGameScene()
	//{
	//	AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

	//	while (!asyncLoad.isDone) yield return null;

	//	yield return new WaitForSecondsRealtime(0.5f);

	//	PlayerConnection.LocalPlayer.SpawnShip();
	//}

	//public IEnumerator coLoadHangarScene()
	//{
	//	yield return null;
	//}
}

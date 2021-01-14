using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
	// Global Members
	public static GameManager Instance;


	[SyncVar(hook = nameof(OnGameModeUpdated))]
	public GameModeType GameMode = GameModeType.Campaign;


	public override void OnStartServer()
	{
		if (Instance == null) { Instance = this; }
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	#region Client

	[Client]
	public void OnGameModeUpdated(GameModeType oldMode, GameModeType newMode) { }

	[ClientRpc]
	public void RpcStartGame() => StartCoroutine(coStartGame());

	[Client]
	private IEnumerator coStartGame()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

		while (!asyncLoad.isDone)
			yield return null;
	}

	#endregion

	#region Server

	#endregion
}

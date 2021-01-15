using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class LobbyManager : NetworkBehaviour
{
	// Global Members
	public static LobbyManager Instance;

	[SyncVar(hook = nameof(OnGameModeUpdated))]
	public GameModeType GameMode = GameModeType.Campaign;

	private void Awake()
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

	[Command]
	public void StartGame()
	{
		RpcStartGame();
	}

	[ClientRpc]
	public void RpcStartGame()
	{
		StartCoroutine(coStartGame());
	}

	[Client]
	private IEnumerator coStartGame()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

		while (!asyncLoad.isDone)
			yield return null;

		//PlayerConnection.LocalPlayer.SpawnShip();
	}
	#endregion

	#region Server

	#endregion
}

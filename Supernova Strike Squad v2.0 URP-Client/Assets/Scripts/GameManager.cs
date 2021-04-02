using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public enum LobbyState
{
	WaitingToReady,
	ReadyToEnterGame,
	PlayingTransition
}

public class GameManager : NetworkBehaviour
{
	public static GameManager Instance;


	[Header("Editor References")]
	public GameManagerSettings Settings;


	[Header("Keybinds")]
	public KeyCode StartButton = KeyCode.Y;

	private LobbyState lobby = LobbyState.WaitingToReady;

	public override void OnStartClient()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	[Server]
	public void OnMissionComplete(bool victory)
	{
		Debug.Log("YOU WON!!");
	}

	//[Server]
	//public IEnumerator LoadSceneServer(string sceneName)
	//{
	//	string oldScene = SceneManager.GetActiveScene().name;

	//	AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

	//	while (!asyncLoad.isDone)
	//	{
	//		yield return null;
	//	}

	//	SceneManager.UnloadSceneAsync(oldScene);

	//	Rpc_LoadScene(sceneName);
	//}

	//[ClientRpc]
	//public void Rpc_LoadScene(string sceneName)
	//{
	//	if (!isServer)
	//	{
	//		StartCoroutine(LoadSceneClient(sceneName));
	//	}
	//}

	//[Client]
	//public IEnumerator LoadSceneClient(string sceneName)
	//{
	//	AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

	//	while (!asyncLoad.isDone)
	//	{
	//		yield return null;
	//	}
	//}

	#region CLEAN UP
	private void FixedUpdate()
	{
		if (lobby == LobbyState.WaitingToReady)
		{
			var players = FindObjectsOfType<Player>();

			bool canStartGame = true;
			foreach (Player player in players)
			{
				if (player.Ready == false) canStartGame = false;
			}

			if (canStartGame && players.Length > 0)
			{
				Debug.Log("START THE GAME");
				lobby = LobbyState.ReadyToEnterGame;

				// GET PLAYER INFO
				if (isServer)
				{
					foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>())
					{
						int playerID = player.ID;

						Debug.Log($"[{playerID}] Player ");

						Debug.Log($"Ship " + player.Ship.ToString());

						foreach (WeaponTypes weapon in player.Weapons)
						{
							Debug.Log($"Weapons " + weapon.ToString());
						}

					}
				}
			}
		}

		if (lobby == LobbyState.ReadyToEnterGame)
		{
			var networkManager = FindObjectOfType<CustomNetworkManager>();
			networkManager.ServerChangeScene("Gameplay");

			lobby = LobbyState.PlayingTransition;
		}
	}

	#endregion
}

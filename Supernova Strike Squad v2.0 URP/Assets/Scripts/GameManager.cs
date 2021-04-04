using Mirror;
using System.Collections;
using UnityEngine;

public enum LobbyState
{
	WaitingToReady,
	ReadyToEnterGame,
	PlayingTransition,
	InGame
}

public class GameManager : NetworkBehaviour
{
	public static GameManager Instance;

	[Header("Editor References")]
	public GameManagerSettings Settings;

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

	private void Update()
	{
		if (!isServer) return;

		switch (lobby)
		{
			case LobbyState.WaitingToReady:
				HandleWaitingForPlayers();
				break;
			case LobbyState.ReadyToEnterGame:
				HandleEnterGame();
				break;
			case LobbyState.PlayingTransition:
				HandleGameTransition();
				break;

			default:break;
		}
	}

	public void HandleWaitingForPlayers()
	{
		var players = FindObjectsOfType<Player>();

		bool start = true;
		foreach (Player player in players)
		{

			if (player.Ready == false) {
				start = false;
			}

		}

		// If we have at least 1 player
		// And all player are ready
		if (start && players.Length > 0) {
			lobby = LobbyState.ReadyToEnterGame;
		}
	}

	public void HandleEnterGame()
	{
		if (lobby == LobbyState.ReadyToEnterGame)
		{
			foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>()) {
				PrintPlayerInfo(player);
			}

			StartCoroutine(coGameTransition());

			lobby = LobbyState.PlayingTransition;
		}
	}

	public void HandleGameTransition()
	{
		if (lobby == LobbyState.PlayingTransition)
		{
			lobby = LobbyState.InGame;
		}
	}

	private IEnumerator coGameTransition()
	{
		bool wait = true;

		// Wait for fade
		Rpc_FadeInLoadingScreen(false);
		LoadingScreen.Instance.FadeIn(() => { wait = false; });
		while (wait) yield return null;

		yield return new WaitForSecondsRealtime(0.5f);

		// Change to the game scene
		var networkManager = FindObjectOfType<CustomNetworkManager>();
		networkManager.ServerChangeScene("Gameplay");

		yield return new WaitForSecondsRealtime(0.5f);

		// Open and start the Node Map
		var nodeMap = FindObjectOfType<NodeMap>();
		nodeMap.StartNodeMap(Settings);
	}

	private IEnumerator coLobbyTransition()
	{
		bool wait = true;

		// Wait for fade in
		Rpc_FadeInLoadingScreen(false);
		LoadingScreen.Instance.FadeIn(() => { wait = false; });
		while (wait) yield return null;

		yield return new WaitForSecondsRealtime(2.5f);

		// Change to the game scene
		var networkManager = FindObjectOfType<CustomNetworkManager>();
		networkManager.ServerChangeScene("Main");

		yield return new WaitForSecondsRealtime(0.5f);

		wait = true;
		Rpc_FadeOutLoadingScreen(false);
		LoadingScreen.Instance.FadeOut(() => { wait = false; });
		while (wait) yield return null;

		foreach (ShipBay ship in FindObjectsOfType<ShipBay>()) {
			foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>())
			{
				if (player.ID == ship.ownerID)
				{
					ship.Open = true;
					continue;
				}
			}
		}

		lobby = LobbyState.WaitingToReady;
	}

	[Server]
	public void OnMissionComplete(bool victory)
	{
		foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>()) {
			player.Ready = false;
		}

		StartCoroutine(coLobbyTransition());
	}

	[ClientRpc] public void Rpc_FadeInLoadingScreen(bool runOnServer ) 
	{
		if (!isServer || runOnServer) LoadingScreen.Instance.FadeIn();
	}
	[ClientRpc] public void Rpc_FadeOutLoadingScreen(bool runOnServer)
	{
		if (!isServer || runOnServer) LoadingScreen.Instance.FadeOut();
	}

	public void PrintPlayerInfo(NetworkPlayer player)
	{
		Debug.Log($"[{player.ID}] Player ");
		Debug.Log($"Ship " + player.Ship.ToString());
		foreach (WeaponTypes weapon in player.Weapons)
		{
			Debug.Log($"Weapons " + weapon.ToString());
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarLobby : MonoBehaviour
{
	// Global Members
	public static HangarLobby Instance;

	public List<Transform> SpawnPoints = new List<Transform>();

	// Editor References
	[SerializeField] 
	private List<Dock> ShipDocks = new List<Dock>();

	// Setup the Hangar Singleton
	void Awake()
	{
		if (Instance) Destroy(gameObject); 
		Instance = this;
	}

	public void UpdateHangarStates()
	{
		foreach (var player in FindObjectsOfType<PlayerConnection>())
		{
			ShipDocks[player.playerID].StopAllCoroutines();
			OpenGate(player.playerID);
		}
	}

	public PlayerInfoDisplay GetInfoDisplay(int playerIndex)
	{
		return ShipDocks[playerIndex].GetInfoDisplay();
	}

	public void OpenGate(int index) => ShipDocks[index]?.OpenDockDoors();
	public void CloseGate(int index) => ShipDocks[index]?.CloseDockDoors();

	public void ChangeGameMode(int newModeID)
	{
		LobbyManager.Instance.GameMode = (GameModeType)newModeID;
	}

	public void StartGame()
	{
		PlayerConnection.LocalPlayer.CmdTransitionFromHangarToGame();
	}
}

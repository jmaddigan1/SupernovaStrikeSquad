using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType
{
	Campaign,
	MissionBoard,
	Endless
}

public class HangarLobby : MonoBehaviour
{
	// Global Members
	public static HangarLobby Instance;

	// Editor References
	[SerializeField] private List<ShipDock> ShipDocks = new List<ShipDock>();
	[SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
	[SerializeField] private List<PlayerInfoDisplay> InfoDisplays = new List<PlayerInfoDisplay>();

	// public GameModeType gameMode;

	// Setup the Hangar Singleton
	void Awake()
	{
		if (Instance) Destroy(gameObject); Instance = this;
	}

	public Transform GetSpawnPosition(int index) => SpawnPoints[index];

	public void UpdateHangarStates()
	{
		foreach (var player in FindObjectsOfType<PlayerConnection>())
		{
			OpenGate(player.playerIndex);
		}
	}

	public PlayerInfoDisplay GetInfoDisplay(int playerIndex)
	{
		return InfoDisplays[playerIndex];
	}

	public void OpenGate(int index) => ShipDocks[index].Open();
	public void CloseGate(int index) => ShipDocks[index].Close();

	public void ChangeGameMode(int newModeID)
	{
		PlayerConnection.LocalPlayer.CmdUpdateGameMode((GameModeType)newModeID);
	}

	public void StartGame()
	{
		PlayerConnection.LocalPlayer.CmdStartGame();
	}

	private void OnDrawGizmos()
	{
		foreach (Transform spawnPoint in SpawnPoints)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
			Gizmos.DrawCube(spawnPoint.position, Vector3.one * 0.5f);
		}
	}
}

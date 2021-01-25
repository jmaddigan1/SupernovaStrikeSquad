using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

// the Lobby Manager manages the state of the players in the hangar
public class LobbyManager : NetworkBehaviour
{
	#region Singleton

	public static LobbyManager Instance;

	private void Awake()
	{
		if (Instance == null) { Instance = this; }
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	#endregion

	public GameModeType GameMode;
	public LobbyType LobbyType;

	#region Server
	#endregion

	#region Client

	void OnGUI()
	{
		GUILayout.BeginVertical("box");
		if (GUILayout.Button("X"))
		{
			EndGame();
		}
		GUILayout.EndVertical();
	}

	public void EndGame() => PlayerConnection.LocalPlayer.CmdTransitionFromGameToHangar(LobbyType == LobbyType.Steam);

	#endregion
}

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


	private void OnGUI()
	{
		float width = 300;

		GUILayout.BeginHorizontal();
		GUILayout.Space( Screen.width -  width);

		GUILayout.BeginVertical("box",GUILayout.Width(width));

		GUILayout.Label("Hangar Lobby");
		GUILayout.BeginVertical("box");

		GUILayout.Label("Lobby: " + LobbyType);
		GUILayout.Label("Mode: " + GameMode);

		if (GameMode == GameModeType.Campaign)
		{
			if (GUILayout.Button("Chapter 1")) { }
			if (GUILayout.Button("Chapter 2")) { }
			if (GUILayout.Button("Chapter 3")) { }
		}
		if (GameMode == GameModeType.MissionBoard)
		{

		}
		if (GameMode == GameModeType.Endless)
		{

		}

		GUILayout.EndVertical();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	#region Server
	#endregion

	#region Client

	//void OnGUI()
	//{
	//	GUILayout.BeginVertical("box");
	//	if (GUILayout.Button("X"))
	//	{
	//		EndGame();
	//	}
	//	GUILayout.EndVertical();
	//}

	public void EndGame() => PlayerConnection.LocalPlayer.CmdTransitionFromGameToHangar(LobbyType == LobbyType.Steam);

	#endregion
}

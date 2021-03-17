using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

// the Lobby Manager keeps track of the players in this lobby and data we need to keep track of between scene loads
public class LobbyManager : NetworkBehaviour
{
	public static LobbyManager Instance;

	#region Singleton

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

	// The Game Mode / NodeMap Type we are going to load
	public GameModeType GameMode;

	// This keep track if we entered through steam or a local lobby
	public LobbyType LobbyType;
}

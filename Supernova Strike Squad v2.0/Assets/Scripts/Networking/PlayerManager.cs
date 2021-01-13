using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
	// Global Members
	public static PlayerManager Instance;


	// Public Members
	[SyncVar]
	public List<PlayerConnection> Players = new List<PlayerConnection>();


	// Setup the Hangar Singleton
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void AddPlayer(PlayerConnection player)
	{
		if (isServer == false) return;

		// Check if this player is already in the player list
		if (Players.Contains(player) == false)
		{
			// If we can simply  add a player to the player list
			if (Players.Count <= 6)
			{
				Players.Add(player);
				player.playerIndex = Players.Count;

				HangarLobby.Instance.OpenGate(player);
			}
			else
			{
				// Else we want to check if there is an open spot
				for (int index = 0; index < Players.Count; index++)
				{
					if (Players[index] == null)
					{
						Players.Add(player);
						player.playerIndex = Players.Count;

						HangarLobby.Instance.OpenGate(player);

						return;
					}

					Debug.Log("ERROR: There is no open sockets for a new player");
				}
			}
		}
	}
	public void RemovePlayer(PlayerConnection player)
	{
		if (isServer == false) return;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamNetworkManager : NetworkManager
{
	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		base.OnServerAddPlayer(conn);

		// Find the new players PlayerConnection script and set its index
		var playerConnection = conn.identity.GetComponent<PlayerConnection>();
		playerConnection.playerIndex = numPlayers - 1;


		//// Steam
		//if (GetComponent<SteamManager>() == null) return;

		//// Get the new players steam ID
		//CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyID, numPlayers - 1);

		//// If we are using steam we want to set this players username and image
		//var playerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();
		//playerInfoDisplay?.SetSteamID(steamID.m_SteamID);
	}

}

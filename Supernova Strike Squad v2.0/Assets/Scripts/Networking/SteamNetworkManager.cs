using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamNetworkManager : NetworkManager
{
	private Stack<int> openIDs = new Stack<int>();

	public override void OnStartServer() {
		for (int index = 5; index >= 0; index--) openIDs.Push(index);
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		base.OnServerAddPlayer(conn);

		// Find the new players PlayerConnection script and set its index
		var playerConnection = conn.identity.GetComponent<PlayerConnection>();
		playerConnection.SetPlayerIndex(openIDs.Pop());

		//// Steam
		//if (GetComponent<SteamManager>() == null) return;

		//// Get the new players steam ID
		//CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyID, playerConnection.playerIndex);

		//// If we are using steam we want to set this players username and image
		//var playerInfoDisplay = HangarLobby.Instance.GetInfoDisplay(playerConnection.playerIndex);
		//playerInfoDisplay?.SetSteamID(steamID.m_SteamID);
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{     
		// Find the players PlayerConnection script and free its index
		var playerConnection = conn.identity.GetComponent<PlayerConnection>();
		openIDs.Push(playerConnection.playerIndex);

		base.OnServerDisconnect(conn);
	}
}

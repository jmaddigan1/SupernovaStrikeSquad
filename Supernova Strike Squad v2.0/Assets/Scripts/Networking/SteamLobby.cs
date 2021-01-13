using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SteamLobby : MonoBehaviour
{
	// Editor References
	// This is a prefab we use to instantiate a button the player can use to host a lobby
	[SerializeField] private GameObject buttonPrefab = null;


	// Steam Callbacks
	protected Callback<LobbyCreated_t> lobbyCreated;
	protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequest;
	protected Callback<LobbyEnter_t> lobbyEntered;


	// Constant Members
	private const string HostAddressKey = "HostAddress";


	// Global Members
	public static CSteamID LobbyID { get; private set; }


	// Private Members
	private NetworkManager networkManager;
	private Transform button;

	void Start()
	{
		button = Instantiate(buttonPrefab, FindObjectOfType<Canvas>().transform).transform;

		if (button.TryGetComponent<Button>(out Button b))
		{
			b.onClick.AddListener(HostLobby);
		}

		networkManager = GetComponent<NetworkManager>();

		if (SteamManager.Initialized)
		{
			lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
			lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
			lobbyJoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequest);
		}
	}

	/// <summary>
	/// Host a Steam Lobby
	/// </summary>
	public void HostLobby()
	{
		button?.gameObject.SetActive(false);

		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
	}

	void OnLobbyCreated(LobbyCreated_t callback)
	{
		// If the lobby was NOT successfulness created we want to activate the button again
		if (callback.m_eResult != EResult.k_EResultOK) { button.gameObject.SetActive(true); }
		else
		{
			LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

			networkManager.StartHost();

			SteamMatchmaking.SetLobbyData(LobbyID, HostAddressKey, SteamUser.GetSteamID().ToString());
		}
	}

	void OnLobbyJoinRequest(GameLobbyJoinRequested_t callback)
	{
		SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
	}

	void OnLobbyEntered(LobbyEnter_t callback)
	{
		if (NetworkServer.active) return;
		else
		{
			string hostAddress = SteamMatchmaking.GetLobbyData(
				new CSteamID(callback.m_ulSteamIDLobby),
				HostAddressKey);

			networkManager.networkAddress = hostAddress;
			networkManager.StartClient();

			button.gameObject.SetActive(false);
		}
	}
}

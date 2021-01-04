using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SteamLobby : MonoBehaviour
{
	[SerializeField] private GameObject buttonPrefab = null;

	protected Callback<LobbyCreated_t> lobbyCreated;
	protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequest;
	protected Callback<LobbyEnter_t> lobbyEntered;

	private const string HostAddressKey = "HostAddress";

	private NetworkManager networkManager;

	private Transform button = null;

	private void Start()
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

	public void HostLobby()
	{
		button.gameObject.SetActive(false);

		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
	}

	public void OnLobbyCreated(LobbyCreated_t callback)
	{
		if (callback.m_eResult != EResult.k_EResultOK) { button.gameObject.SetActive(true); }
		else
		{
			networkManager.StartHost();

			SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
		}
	}

	public void OnLobbyJoinRequest(GameLobbyJoinRequested_t callback)
	{
		SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
	}

	public void OnLobbyEntered(LobbyEnter_t callback)
	{
		if (NetworkServer.active) return;
		else
		{
			string hostAddress = SteamMatchmaking.GetLobbyData (
				new CSteamID(callback.m_ulSteamIDLobby),
				HostAddressKey);

			networkManager.networkAddress = hostAddress;
			networkManager.StartClient();

			button.gameObject.SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

// A LobbyItem is the panel that represents a player in the lobby list
public class LobbyItem : NetworkBehaviour
{
	public static LobbyItem LocalItem;

	//
	[SerializeField] private GameObject settingsPrefab = null;

	//
	[SerializeField] private TextMeshProUGUI nameText = null;

	//
	[SerializeField] private GameObject editButton = null;

	// 
	[SerializeField] private Image playerColor = null;
	
	//
	[SerializeField] private Image playerReady = null;


	[SyncVar(hook ="OnReady")]
	public bool Ready;

	void Awake()
	{
		transform.SetParent(Lobby.Instance.Anchor);
	}

	public override void OnStartAuthority()
	{
		LocalItem = this;
		CmdOnNewConnectionInfo();
		editButton.SetActive(true);
	}

	public void OpenSettings()
	{
		Instantiate(settingsPrefab, FindObjectOfType<Canvas>().transform);
	}

	//
	[Command]
	public void CmdOnNewConnectionInfo()
	{
		foreach (Player player in FindObjectsOfType<Player>())
		{
			player.Lobby.InfoPanel.RpcUpdateInfo(player.ID, player.Username, player.Color);
		}
	}

	//
	[ClientRpc]
	public void RpcUpdateInfo(int ID, string username, Color color)
	{
		nameText.text = username;
		playerColor.color = color;
	}

	[Client]
	public void UpdateReady(bool state)
	{
		Cmd_UpdateReady(state);
	}

	[Command]
	private void Cmd_UpdateReady(bool state)
	{
		Ready = state;
	}

	public void OnReady(bool oldState, bool newState)
	{
		playerReady.color = newState ? Color.green : Color.red;
	}
}

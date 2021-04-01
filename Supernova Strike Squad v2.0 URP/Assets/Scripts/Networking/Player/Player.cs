using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(PlayerLobby))]
[RequireComponent(typeof(PlayerChat))]
[RequireComponent(typeof(PlayerID))]

public abstract class Player : NetworkBehaviour
{
	// Static Members
	// The Local Player for this client
	public static Player LocalPlayer;


	// The number of player connected to the server
	public static int PlayerCount = 0;


	// Editor References
	// The Players Chat Manager Component
	[SerializeField] private PlayerChat chat = null;

	// The Players Lobby Manager Component
	[SerializeField] private PlayerLobby lobby = null;

	// The Players ID Manager Component
	[SerializeField] private PlayerID playerID = null;


	// Public Members
	// This players username
	[SyncVar(hook = "OnUsernameChange")] public string Username;
	public virtual void OnUsernameChange(string oldUsername, string newUsername) { }

	// This players Color
	[SyncVar(hook = "OnColorChange")] public Color Color;
	public virtual void OnColorChange(Color oldColor, Color newColor) { }


	// Properties
	// Get this players ID
	public int ID { get { return playerID.ID; } set { playerID.ID = value; } }

	//
	public bool Ready = false;

	// Get this players Lobby Manager
	public PlayerLobby Lobby { get { return lobby; } }

	// Get this players Chat Manager
	public PlayerChat Chat { get { return chat; } }

	// The Player
	public NetworkPlayer Self { get { return GetComponent<NetworkPlayer>(); } }


	// Methods
	// OnStart and OnStop a Client
	public override void OnStartClient()
	{
		DontDestroyOnLoad(gameObject);
		PlayerCount++;
	}
	public override void OnStopClient()
	{
		PlayerCount--;
	}

	public virtual void OnIDChange(int oldID, int newID) { }

	public override void OnStartAuthority()
	{
		LocalPlayer = this;

		Lobby.CmdSpawnConnectionPanel();
	}


	[Command]
	public void Cmd_UpdateUsername(string newUsername) {
		Username = newUsername;
	}

	[Command]
	public void Cmd_UpdateColor(Color newColor) {
		Color = newColor;
	}

	[Command]
	public void Cmd_UpdateReady(bool ready) {
		Ready = ready;
	}

	#region GUI

	public void DrawPlayerGUI()
	{
		//GUIStyle contentStyle = new GUIStyle();

		//contentStyle.alignment = TextAnchor.MiddleLeft;
		//contentStyle.fontStyle = FontStyle.Normal;

		//contentStyle.normal.textColor = Color.white;

		//GUILayout.BeginVertical("box");
		//GUILayout.Label($"[{ID}] {Username}", contentStyle);
		//GUILayout.Label($"Ready: {Ready}", contentStyle);
		//GUILayout.Label($"Ship: {Self.Ship.ToString()}", contentStyle);
		//foreach (WeaponTypes weapon in Self.Weapons)
		//{
		//	GUILayout.Label($"Weapon: {weapon.ToString()}", contentStyle);
		//}
		//GUILayout.EndVertical();
	}
	#endregion
}

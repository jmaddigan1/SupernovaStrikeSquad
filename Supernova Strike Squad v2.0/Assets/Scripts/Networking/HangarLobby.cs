using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarLobby : MonoBehaviour
{
	// Global Members
	public static HangarLobby Instance;


	// Editor References
	[SerializeField]
	private List<ShipDock> ShipDocks = new List<ShipDock>();


	// Setup the Hangar Singleton
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void OpenGate(PlayerConnection player)
	{
		ShipDocks[player.playerIndex].Open();
	}
}

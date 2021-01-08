using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Menu startingMenu;


	public static UIManager Instance;


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
	}
	void Start()
	{
		if (startingMenu) startingMenu.Open();
	}

	public void LoadHangarScene(bool onlineMode)
	{
		StartCoroutine(coLoadHangarScene(onlineMode));
	}

	IEnumerator coLoadHangarScene(bool onlineMode)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Hangar");

		while (!asyncLoad.isDone)
			yield return null;

		NetworkSwitch networkSwitch = FindObjectOfType<NetworkSwitch>();

		if (networkSwitch == null)
		{
			// ERROR
		}
		else
		{
			networkSwitch.LobbyType = onlineMode ? NetworkLobbyType.SteamLobby : NetworkLobbyType.LocalLobby;

			networkSwitch.GenerateNetworkController();

			if (onlineMode)
			{
				SteamLobby networkManager = FindObjectOfType<SteamLobby>();
				//if (networkManager) networkManager.HostLobby();
			}
			else
			{
				NetworkManager manager = FindObjectOfType<NetworkManager>();
				if (manager) manager.StartHost();
			}
		}
	}

	public void Quit() => Application.Quit();
}

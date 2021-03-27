using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
	public static GameManager Instance;


	[Header("Editor References")]
	public GameManagerSettings Settings;


	[Header("Keybinds")]
	public KeyCode StartButton = KeyCode.Y;


	[Header("Players")]
	public List<ShipBay> Ships = new List<ShipBay>();

	public override void OnStartClient()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(StartButton)) StartGame();
	}

	public void StartGame() => StartCoroutine(coTestGame());

	IEnumerator coTestGame()
	{
		foreach (ShipBay ship in Ships)
		{
			Debug.Log("SHIP LOADING");
			Debug.Log(ship.ownerID);
		}

		yield return coChangeScene("Gameplay");
		yield return coSimulateGame();

		yield return new WaitForSecondsRealtime(1.5f);

		yield return coChangeScene("Main");
	}
	IEnumerator coChangeScene(string scene)
	{
		PlayerController.Interacting = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		bool wait = true;
		LoadingScreen.Instance.FadeIn(() => { wait = false; });
		while (wait)
		{
			yield return null;
		}

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		wait = true;
		LoadingScreen.Instance.FadeOut(() => { wait = false; });
		while (wait)
		{
			yield return null;
		}

		PlayerController.Interacting = false;
	}
	IEnumerator coSimulateGame()
	{
		print("Starting Game");
		yield return new WaitForSecondsRealtime(0.5f);

		print("playing Level");
		yield return new WaitForSecondsRealtime(0.5f);

		print("Getting Reward");
		yield return new WaitForSecondsRealtime(0.5f);

		print("Ending Game");
		yield return new WaitForSecondsRealtime(0.5f);

		print("Changing Scene to Hangar");
		yield return new WaitForSecondsRealtime(0.5f);
	}
}

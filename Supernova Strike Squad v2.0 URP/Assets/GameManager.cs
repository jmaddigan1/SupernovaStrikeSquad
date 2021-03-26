using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public KeyCode StartButton = KeyCode.Y;

	public List<ShipBay> Ships = new List<ShipBay>();

	void Awake()
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

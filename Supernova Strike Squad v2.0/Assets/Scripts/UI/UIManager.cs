using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Menu startingMenu = null;


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

	// Load a single player scene
	public void LoadHangar_Local() => StartCoroutine(coLoadHangar_Local());
	IEnumerator coLoadHangar_Local()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Hangar_Local");

		while (!asyncLoad.isDone)
			yield return null;		
	}

	// Load the Steam Lobby scene
	public void LoadHanger_Online() => StartCoroutine(coLoadHanger_Online());
	IEnumerator coLoadHanger_Online()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Hangar_Steam");

		while (!asyncLoad.isDone)
			yield return null;
	}

	public void Quit() => Application.Quit();
}

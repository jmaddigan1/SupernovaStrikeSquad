using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
	// Load the hangar in local / single player mode
	public void Play()
	{
		LoadingScreen.Instance.FadeIn(LoadHangar);
		Destroy(gameObject);
	}

	// Load the hangar in online mode
	public void Online()
	{
		LoadingScreen.Instance.FadeIn(LoadHangar);
		Destroy(gameObject);
	}


	// Load the hangar in its corresponding mode
	void LoadHangar()
	{
		currentMenu = null;

		SceneManager.LoadScene("Main");

		LoadingScreen.Instance.FadeOut();
	}
}

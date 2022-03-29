﻿using System.Collections;
using System.Collections.Generic;
using Supernova.Managers;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class MainMenu : Menu
{
	// Load the hangar in local / single player mode
	public void Play() {
		//LoadingScreen.Instance.FadeIn(LoadHangar);
		//Destroy(gameObject);
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
		SceneManager.LoadScene("Main");
		LoadingScreen.Instance.FadeOut();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
	[SerializeField] private ConfirmationMenu ConfirmationMenu = null; 

	public void Play()
	{
		Instantiate(ConfirmationMenu, transform.parent).Open("Play Alone", (bool result) => {
			if (result == true)
			{
				LoadingScreen.Instance.FadeIn(LoadHangar);
				Destroy(gameObject);
			}
		});

	}
	
	public void Online()
	{
		Instantiate(ConfirmationMenu, transform.parent).Open("Join a Lobby", (bool result) => {
			if (result == true)
			{
				LoadingScreen.Instance.FadeIn(LoadHangar);
				Destroy(gameObject);
			}
		});
	}

	void LoadHangar()
	{
		Debug.Log("test");

		currentMenu = null;
		SceneManager.LoadScene("Main");
		LoadingScreen.Instance.FadeOut();
	}
}

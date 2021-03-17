using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public static LoadingScreen Instance;

	[SerializeField] private Image blocker = null;

	void Awake()
	{
		if (Instance == null) { Instance = this; }
		else
		{
			Destroy(this);
		}

		transform.SetParent(null); 

		DontDestroyOnLoad(gameObject);
	}

	[ContextMenu("Open")]
	public void OpenScreen()
	{
		StopAllCoroutines();
		StartCoroutine(Fade(true));
	}

	[ContextMenu("Close")]
	public void CloseScreen()
	{
		StopAllCoroutines();
		StartCoroutine(Fade(false));
	}

	IEnumerator Fade(bool fadeIn)
	{
		Color target = fadeIn ? Color.black : Color.clear;
		Color current = !fadeIn ? Color.black : Color.clear;

		float duration = 0.75f;
		float time = 0.0f;

		while (time < duration)
		{
			time += Time.deltaTime;

			blocker.color = Color.Lerp(current, target, time / duration);
			yield return null;
		}
	}
}

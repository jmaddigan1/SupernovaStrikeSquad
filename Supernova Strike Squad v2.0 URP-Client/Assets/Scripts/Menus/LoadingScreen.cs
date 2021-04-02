using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public static LoadingScreen Instance;

	[SerializeField] private Slider progressSlider = null;
	[SerializeField] private Image blocker = null;

	void Awake()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(GetComponentInParent<Canvas>().gameObject);
		}

		FadeOut();
	}

	[ContextMenu("FadeIn")]
	public void FadeIn(Action action = null)
	{
		blocker.raycastTarget = true;
		//progressSlider.gameObject.SetActive(true);
		StartCoroutine(Fade(true, action));
	}

	[ContextMenu("FadeOut")]
	public void FadeOut(Action action = null)
	{
		blocker.raycastTarget = false;
		//progressSlider.gameObject.SetActive(false);
		StartCoroutine(Fade(false, action));
	}

	public IEnumerator Fade(bool fadeIn, Action action)
	{
		Color target = fadeIn ? Color.black : Color.clear;
		Color current = blocker.color;

		float time = 0.0f;
		float duration = 1.0f;

		do
		{
			time = (time + Time.deltaTime);
			blocker.color = Color.Lerp(current, target, time / duration);
			yield return null;

		} while (time < duration);

		yield return new WaitForSeconds(1.0f);

		action?.Invoke();
	}
}

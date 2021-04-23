using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CustomAudioSources : MonoBehaviour
{
	[SerializeField] private GameSettings settings = null;

	AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = settings.AmbientVolume;
	}
}

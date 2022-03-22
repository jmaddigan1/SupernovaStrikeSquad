using System.Collections;
using System.Collections.Generic;
using Supernova.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	[SerializeField] private AudioClip hoverClip = null;
	[SerializeField] private AudioClip clickClip = null;

	[SerializeField] private GameSettings settings = null;

	public void OnPointerDown(PointerEventData eventData)
	{
		AudioSource.PlayClipAtPoint(clickClip, CameraManager.Instance.Current.transform.position, settings.UIVolume * settings.MasterVolume);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		AudioSource.PlayClipAtPoint(hoverClip, CameraManager.Instance.Current.transform.position, settings.UIVolume * settings.MasterVolume);
	}
}

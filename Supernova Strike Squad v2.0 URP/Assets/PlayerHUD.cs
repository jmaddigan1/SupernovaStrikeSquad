using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	[SerializeField] private GameObject playerShip = null;

	[SerializeField] private Slider healthSlider = null;
	[SerializeField] private Slider shieldSlider = null;

	private void Start()
	{
		if (playerShip.TryGetComponent<Health>(out Health Health))
		{
			Health.OnShieldUpdate += OnShieldUpdate;
			Health.OnHealthUpdate += OnHealthUpdate;
		}
	}

	private void OnDestroy()
	{
		if (playerShip.TryGetComponent<Health>(out Health Health))
		{
			Health.OnShieldUpdate -= OnShieldUpdate;
			Health.OnHealthUpdate -= OnHealthUpdate;
		}
	}

	public void OnHealthUpdate(float value, float maxValue)
	{
		healthSlider.value = value / maxValue;
	}

	public void OnShieldUpdate(float value, float maxValue)
	{
		shieldSlider.value = value / maxValue;
	}
}

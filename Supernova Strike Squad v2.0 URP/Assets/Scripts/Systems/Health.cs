using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour, IDamageable
{
	public float MaxHealth = 10;
	public float MaxShield = 0;

	[SyncVar(hook = "OnHealthChange"), SerializeField]
	float currentHealth = 0;

	[SyncVar(hook = "OnShieldChange"), SerializeField]
	float currentShield = 0;

	public override void OnStartServer()
	{
		currentHealth = MaxHealth;
		currentShield = MaxShield;
	}

	public void TakeDamage(float damage)
	{
		if (!isServer) return;

		if (currentShield <= 0)
		{
			// Damage to my health
			//

			currentHealth -= damage;

			if (currentHealth <= 0)
			{
				OnDeath();
			}
		}
		else
		{
			// Damage to my Shield
			//

			float newShieldValue = currentShield - damage;
			float overflowDamage = Mathf.Abs(newShieldValue);

			if (newShieldValue < 0)
			{
				currentShield = 0;
				TakeDamage(overflowDamage);
			}
			else
			{
				currentShield = currentShield - damage;
			}
		}
	}

	public void OnDeath()
	{
		NetworkServer.Destroy(gameObject);
	}

	public void OnHealthChange(float oldValue, float newValue)
	{
		Color R = new Color(1, 0, 0, 0.2f);
		Color W = new Color(1, 1, 1, 0.2f);

		Color newColor = Color.Lerp(R, W, newValue / MaxHealth);
		ColorModel(newColor);
	}

	public void OnShieldChange(float oldValue, float newValue)
	{
		Color B = new Color(0, 0, 1, 0.2f);
		Color W = new Color(1, 1, 1, 0.2f);

		Color newColor = Color.Lerp(B, W, newValue / MaxHealth);
		ColorModel(newColor);
	}

	void ColorModel(Color color)
	{
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			renderer.material.color = color;
		}
	}
}

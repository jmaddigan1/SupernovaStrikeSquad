﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour, IDamageable
{
	[SerializeField]
	private Renderer shieldRenderer = null;

	public float MaxHealth = 10;
	public float MaxShield = 0;

	public float RechargeTimer = 0;
	public float RechargeDelay = 2;
	public float RechargeSpeed = 2;

	public bool Invincible = false;

	[SyncVar(hook = "OnHealthChange"), SerializeField]
	float currentHealth = 0;

	[SyncVar(hook = "OnShieldChange"), SerializeField]
	float currentShield = 0;

	public OnHealthUpdate OnHealthUpdate;
	public OnShieldUpdate OnShieldUpdate;

	public Color ShieldColor;

	public override void OnStartServer()
	{
		currentHealth = MaxHealth;
		currentShield = MaxShield;
	}

	public override void OnStartClient()
	{
		if (shieldRenderer) {

			MaterialPropertyBlock block = new MaterialPropertyBlock();

			block.SetColor("_ShieldColor", ShieldColor);
			shieldRenderer.SetPropertyBlock(block);
		}
	}

	private void FixedUpdate()
	{
		if (!isServer) return;

		if (MaxShield > 0 && currentShield < MaxShield)
		{
			RechargeTimer += Time.fixedDeltaTime;

			if (RechargeTimer > RechargeDelay) {
				currentShield += RechargeSpeed * Time.fixedDeltaTime;
			}
		}
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

			RechargeTimer = 0;
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

			RechargeTimer = 0;
		}
	}

	private void OnDeath()
	{
		if (!Invincible) NetworkServer.Destroy(gameObject);
	}

	public void OnHealthChange(float oldValue, float newValue)
	{
		OnHealthUpdate?.Invoke(newValue, MaxHealth);

		Color R = new Color(1, 0, 0, 0.2f);
		Color W = new Color(1, 1, 1, 0.2f);

		Color newColor = Color.Lerp(R, W, newValue / MaxHealth);
		ColorModel(newColor);
	}
	public void OnShieldChange(float oldValue, float newValue)
	{
		OnShieldUpdate?.Invoke(newValue, MaxShield);

		Color B = new Color(0, 0, 1, 0.2f);
		Color W = new Color(1, 1, 1, 0.2f);

		Color newColor = Color.Lerp(B, W, newValue / MaxShield);
		ColorModel(newColor);
	}

	void ColorModel(Color color)
	{
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			renderer.material.color = color;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour, IDamageable
{
	public int MaxHealth;

	[SyncVar(hook = nameof(OnCurrentHealthChanged))]
	public int CurrentHealth;

	private void OnCollisionEnter(Collision collision)
	{
		if (isServer)
		{
			if (collision.gameObject.TryGetComponent<IDamageSource>(out IDamageSource source)) {
				DealDamage(source.GetDamage());
			}
		}
	}

	#region Client

	public void OnCurrentHealthChanged(int oldHealth, int newHealth)
	{
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			renderer.material.color = Color.Lerp(Color.red, Color.green, (float)newHealth / MaxHealth);
		}
	}

	#endregion

	#region Server

	public override void OnStartServer()
	{
		CurrentHealth = MaxHealth;
	}

	[Server]
	public void DealDamage(int damage)
	{
		SetHealth(Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth));
	}

	[Server]
	private void SetHealth(int value)
	{
		CurrentHealth = value;

		if (CurrentHealth <= 0)
		{
			OnDeath();
		}
	}

	[Server]
	public void OnDeath() => NetworkServer.Destroy(gameObject);

	#endregion
}

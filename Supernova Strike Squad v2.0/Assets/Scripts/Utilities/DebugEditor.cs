using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mirror;

public class DebugEditor : MonoBehaviour
{
	public const string Box  = "box";

	Vector2 offset = new Vector2(100, 100);
	Vector2 size = new Vector2(350, 600);

	bool showing = true;

	void Start()
	{
		offset.x = Screen.width - size.x;
		offset.y = 0;
	}

	void OnGUI()
	{
		if (showing)
		{
			GUILayout.Space(offset.y);
			GUILayout.BeginHorizontal();

			GUILayout.Space(offset.x);
			GUILayout.BeginVertical(Box, GUILayout.Width(size.x), GUILayout.Height(size.y));

			GUILayout.BeginHorizontal();
			GUILayout.Label("Supernova Strike Squad Editor");

			if (GUILayout.Button("X", GUILayout.Width(25)))
			{
				showing = false;
			}

			GUILayout.EndHorizontal();

			DrawServerStatus();

			DrawWeaponStatus();

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.Space(offset.y);
			GUILayout.BeginHorizontal();

			GUILayout.Space(Screen.width - 40);
			GUILayout.BeginVertical(Box, GUILayout.Width(40), GUILayout.Height(40));

			if (GUILayout.Button("X", GUILayout.Height(40)))
			{
				showing = true;
			}

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}		
	}

	void DrawWeaponStatus()
	{
		var weaponSystems = PlayerConnection.GetLocalPlayersWeaponsSystem();

		if (weaponSystems == null) return;

		GUI.color = new Color(1, 1, 1, 0.4f);

		GUILayout.BeginVertical(Box);

		GUI.color = Color.white;

		GUILayout.Label("Weapon");

		GUILayout.Label($"   {weaponSystems.CurrentWeapon}");
		GUILayout.Label($"   {weaponSystems.CurrentWeapon.Shooting}");

		GUILayout.EndVertical();
	}

	void DrawServerStatus()
	{
		GUI.color = new Color(1, 1, 1, 0.4f);

		GUILayout.BeginVertical(Box);

		GUI.color = Color.white;

		GUILayout.Label("Status");

		GUILayout.Label($"   Server Started: {NetworkServer.active}");
		GUILayout.Label($"   Client Count: { NetworkServer.connections.Count}");

		GUILayout.EndVertical();
	}
}

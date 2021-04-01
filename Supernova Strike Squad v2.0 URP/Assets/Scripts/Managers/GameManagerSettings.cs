using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManagerSettings : NetworkBehaviour
{
	public MissionTypes MissionTypes;

	[Server] public void UpdateMissionType(string[] args)
	{
		MissionTypes missionTypes = (MissionTypes)(int.Parse(args[0]));

		Debug.Log("Gas Authority? " + hasAuthority);

		print($"Mission type changed to: {missionTypes.ToString()}");


		// CAMPAIGN
		if (missionTypes == MissionTypes.Campaign)
		{
			int campaignID = int.Parse(args[1]);

			print($"We have selected a campaign mission with the ID: {campaignID}");
		}

		// MISSIONBOARD
		if (missionTypes == MissionTypes.MissionBoard)
		{
			print($"We have selected a contract mission");
		}

		// ENDLESS
		if (missionTypes == MissionTypes.Endless)
		{
			int startingDepth = int.Parse(args[1]);

			print($"We have selected a endless mission with a starting depth of: {startingDepth}");
		}
	}

	//private void OnGUI()
	//{
	//	GUILayout.BeginVertical();

	//	if (isServer)
	//	{
	//		GUIStyle contentStyle = new GUIStyle();
	//		contentStyle.alignment = TextAnchor.LowerRight;
	//		contentStyle.fontStyle = FontStyle.Normal;
	//		contentStyle.normal.textColor = Color.white;

	//		// PLAYERS
	//		GUILayout.BeginVertical("box", GUILayout.Width(150));
	//		foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>()) {
	//			player.DrawPlayerGUI();
	//		}
	//		GUILayout.EndVertical();
	//	}

	//	GUILayout.Label("Press Enter to ready up! " + "Ready: " + Player.LocalPlayer.Ready);
	//	GUILayout.EndVertical();
	//}
}

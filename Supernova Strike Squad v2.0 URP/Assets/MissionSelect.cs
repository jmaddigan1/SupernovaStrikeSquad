using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionTypes
{
	Campaign,
	MissionBoard,
	Endless
}

public class MissionSelect : MonoBehaviour
{
	public List<MissionPanel> MissionPanels = new List<MissionPanel>();

	bool missionViewMode = false;

	public void SelectMissionType(MissionTypes missionType)
	{
		foreach (MissionPanel missionPanel in MissionPanels) {
			missionPanel.CanBeClicked = false;
		}

		foreach (MissionPanel missionPanel in MissionPanels)
		{
			if (missionPanel.MissionType == missionType)
			{
				missionPanel.TargetWidth = 10000;
			}
			else
			{
				missionPanel.TargetWidth = 0;
			}

			missionViewMode = true;
		}
	}

	public void Return()
	{
		if (missionViewMode)
		{
			foreach (MissionPanel missionPanel in MissionPanels)
			{
				missionPanel.CanBeClicked = true;
				missionPanel.RecalculateWidth();
			}
		}
		else
		{
			Debug.Log("Exit out of mission select");
		}
	}
}

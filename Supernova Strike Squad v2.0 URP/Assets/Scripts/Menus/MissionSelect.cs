using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionTypes
{
	None,
	Campaign,
	MissionBoard,
	Endless
}

public class MissionSelect : Menu
{
	public List<MissionPanel> MissionPanels = new List<MissionPanel>();

	private bool missionViewMode = false;

	private void Start()
	{
		UpdateCursor(CursorLockMode.None, true);
	}

	private void Update()
	{
		// If the player is interacting with a menu
		if (PlayerController.Interacting)
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				CloseMenu();
			}
		}
	}

	public override void CloseMenu()
	{
		if (missionViewMode)
		{
			foreach (MissionPanel missionPanel in MissionPanels)
			{
				missionPanel.CanBeClicked = true;
				missionPanel.RecalculateWidth();
				missionPanel.HideContent();
			}

			missionViewMode = false;
		}
		else
		{
			UpdateCursor(CursorLockMode.Locked, false);

			base.CloseMenu();
		}
	}

	public void SelectMissionType(MissionTypes missionType)
	{
		foreach (MissionPanel missionPanel in MissionPanels)
		{
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

	}
}

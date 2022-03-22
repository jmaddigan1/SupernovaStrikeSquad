using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectMissionButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		string[] data = new string[] { ((int)MissionTypes.MissionBoard).ToString() };
		Player.LocalPlayer.Self.Cmd_UpdateMissionType(data);
	}
}

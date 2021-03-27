using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectEndlessButton : MonoBehaviour, IPointerClickHandler
{
	public int StartingDepth = 0; 

	public void OnPointerClick(PointerEventData eventData)
	{
		string[] data = new string[] { ((int)MissionTypes.Endless).ToString(), StartingDepth.ToString() };
		Player.LocalPlayer.Self.Cmd_UpdateMissionType(data);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class SelectCampaignButton : MonoBehaviour, IPointerClickHandler
{
	public string CampaignID;

	private void Start()
	{
		var nameText = GetComponentInChildren<TextMeshProUGUI>().text = CampaignID;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		string[] data = new string[] { ((int)MissionTypes.Campaign).ToString(), CampaignID };
		Player.LocalPlayer.Self.Cmd_UpdateMissionType(data);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MissionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private LayoutElement layoutElement = null;
	[SerializeField] private Transform panelContent = null;

	public MissionTypes MissionType;

	private float preferredWidth = 0;
	private float selectedWidth = 0;

	public float TargetWidth = 0;

	public bool CanBeClicked = true;

	void Start()
	{
		RecalculateWidth();
	}

	void Update()
	{
		layoutElement.preferredWidth = Mathf.Lerp(layoutElement.preferredWidth, TargetWidth, Time.deltaTime * 4);
	}

	public void RecalculateWidth()
	{
		var parent = transform.parent.GetComponent<RectTransform>();

		preferredWidth = parent.rect.width / 3;
		selectedWidth = parent.rect.width / 2f;

		TargetWidth = preferredWidth;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (CanBeClicked)
			TargetWidth = selectedWidth;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		if (CanBeClicked)
			TargetWidth = preferredWidth;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (CanBeClicked)
		{
			var missionSelect = GetComponentInParent<MissionSelect>();

			if (missionSelect)
			{
				missionSelect.SelectMissionType(MissionType);
				ShowContent();
			}
		}		
	}

	public void ShowContent()
	{
		panelContent.gameObject.SetActive(true);
		Tween.Instance.EaseOut_Scale_QuartX(panelContent, 0, 1, 0.5f);
	}

	public void HideContent()
	{
		Tween.Instance.EaseOut_Scale_QuartX(panelContent, 1, 0, 0.5f, 0, () => {
			panelContent?.gameObject.SetActive(false);
		});
	}
}

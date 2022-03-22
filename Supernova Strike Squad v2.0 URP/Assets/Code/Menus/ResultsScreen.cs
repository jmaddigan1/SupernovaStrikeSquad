using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ResultsScreenTypes
{
	Starting,
	Victory,
	Failure
}

public class ResultsScreen : Menu
{
	public static ResultsScreen Instance;

	public ResultsScreenTypes testType;

	[SerializeField] private TextMeshProUGUI resultsText = null;

	[SerializeField] private RectTransform resultsPanel = null;
	[SerializeField] private RectTransform backgroundImage = null;

	void Awake()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(GetComponentInParent<Canvas>().gameObject);
		}
	}

	public void DisplayResultsMessage(ResultsScreenTypes resultsScreenTypes)
	{
		switch (resultsScreenTypes)
		{
			case ResultsScreenTypes.Starting: OnStarting(); break;

			case ResultsScreenTypes.Victory: OnVictory(); break;
			case ResultsScreenTypes.Failure: OnFailure(); break;

			default: break;
		}

		Tween.Instance.EaseOut_Transform_QuartX(resultsPanel, 0, backgroundImage.rect.width, 1.2f, 1, () => {
			Tween.Instance.EaseIn_Transform_QuartX(resultsPanel, backgroundImage.rect.width, backgroundImage.rect.width * 2, 0.65f, 3.0f);
		});
	}

	public void OnStarting() {
		resultsText.text = "| Beginning new Campaign |";
	}

	public void OnVictory()	{
		resultsText.text = "| Victory! |";
	}

	public void OnFailure()	{
		resultsText.text = "| Failure |";
	}
}

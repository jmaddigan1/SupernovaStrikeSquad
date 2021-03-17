using System.Collections;
using UnityEngine;

public class Dock : MonoBehaviour
{
	[Header("References")]

	[SerializeField]
	private Transform dockDoorL = null;

	[SerializeField]
	private Transform dockDoorR = null;

	[SerializeField]
	private Transform shipModel = null;

	[Header("References")]

	public PlayerInfoDisplay InfoDisplay = null;

	[Header("States")]

	public DockState State = DockState.Closed;

	public PlayerInfoDisplay GetInfoDisplay() => InfoDisplay;

	[ContextMenu("Open Dock")]
	public void OpenDockDoors()
	{
		StartCoroutine(coOpenDockDoors());
	}

	[ContextMenu("Close Dock")]
	public void CloseDockDoors()
	{
		StartCoroutine(coCloseDockDoors());
	}

	private IEnumerator coOpenDockDoors()
	{
		if (State == DockState.Transitioning || State == DockState.Open) yield return coCloseDockDoors();

		if (State == DockState.Closed)
		{
			State = DockState.Transitioning;

			bool waiting = true;

			Tween.Instance.EaseOut_Scale_BounceX(dockDoorL, 1, 0.25f, 3, 0);
			Tween.Instance.EaseOut_Scale_BounceX(dockDoorR, 1, 0.25f, 3, 0, () => {
				waiting = false;
			});

			while (waiting) yield return null;

			waiting = true;

			Tween.Instance.EaseOut_Transform_ElasticY(shipModel, 0, 3, 1.5f, 0, () => {
				waiting = false;
			});

			while (waiting) yield return null;

			State = DockState.Open;
		}
	}
	private IEnumerator coCloseDockDoors()
	{
		State = DockState.Transitioning;

		bool waiting = true;

		Tween.Instance.EaseOut_Transform_ElasticY(shipModel, shipModel.localPosition.y, 0, 2.5f, 0, () => {
			waiting = false;
		});

		while (waiting) yield return null;

		waiting = true;

		Tween.Instance.EaseOut_Scale_BounceX(dockDoorL, 0.25f, 1, 2.5f, 0);
		Tween.Instance.EaseOut_Scale_BounceX(dockDoorR, 0.25f, 1, 2.5f, 0, () => {
			waiting = false;
		});

		while (waiting) yield return null;

		State = DockState.Closed;
	}
}

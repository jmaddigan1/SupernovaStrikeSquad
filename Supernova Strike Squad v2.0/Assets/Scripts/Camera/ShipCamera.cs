using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipCamera : MonoBehaviour
{
	public static ShipCamera Instance;

	public Transform Target;
	public Transform Cam;

	public Compass Compass;

	public bool playingAnimation;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}

		Instance = this;
	}

	public void SetTarget(Transform target, Vector3 offset)
	{
		Target = target;

		//foreach (PlayerShipController ship in FindObjectsOfType<PlayerShipController>())
		//{
		//	Compass.AddTarget(ship.transform);
		//}

		transform.SetParent(target);
		transform.localPosition = offset;
	}

	void Update()
	{
		// Blocker
		if (Target = null) return;


		if (!playingAnimation)
		{

			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			float scale = 0.5f;

			float camPosX = (input.x * (2.5f * scale));
			float camPosY = (input.y * (1.0f * scale)) + 0.5f;

			float camPosZ = -6.5f;

			transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(camPosX, camPosY, camPosZ), Time.deltaTime * 4);

		}
	}

	public void PlayEnterLevel()
	{
		playingAnimation = true;

		Tween.Instance.EaseOut_Transform_ElasticZ(transform, -25, -10.5f, 2f, 0, () => { playingAnimation = false; });
		Tween.Instance.EaseOut_Transform_QuartY(transform, 25, 0.5f, 1f, 0);
	}
	public void PlayExitLevel()
	{
		playingAnimation = true;

		Tween.Instance.EaseIn_Transform_ElasticZ(transform, -6.5f, 45, 2.5f, 0, () => { playingAnimation = false; });
		Tween.Instance.EaseIn_Transform_ElasticY(transform, 0.5f, 3.5f, 2f, 0);
	}
}

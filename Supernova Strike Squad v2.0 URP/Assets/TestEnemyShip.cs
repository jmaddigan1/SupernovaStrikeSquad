using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class TestEnemyShip : MonoBehaviour
{
	public Transform Target;

	public float RotationSpeed = 0.5f;
	public float RotationMultiplier = 1f;

	Rigidbody rigidbody;

	// Start is called before the first frame update
	IEnumerator Start()
	{
		while (Target == null)
		{
			Target = FindObjectOfType<ShipController>().transform;
			yield return null;
		}

		rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		float rotationSpeed = 1f;
		Quaternion neededRotation = Quaternion.LookRotation(Target.position - transform.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, neededRotation, Time.deltaTime * rotationSpeed * RotationMultiplier);

		////find the vector pointing from our position to the target
		//_direction = (Target.position - transform.position).normalized;

		////create the rotation we need to be in to look at the target
		//_lookRotation = Quaternion.LookRotation(_direction);

		////rotate us over time according to speed until we are in the required rotation
		//transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

		//Vector3 targetDir = Target.position - transform.position;
		//float angle = Vector3.Angle(targetDir, transform.forward);

		//if (angle > 1)
		//{
		//	RotationMultiplier = Mathf.Lerp(RotationMultiplier, 5f, Time.deltaTime * 1);
		//}
		//else
		//{
		//	// Else we are in a shooting angle
		//	RotationMultiplier = RotationMultiplier = 0.5f;
		//}

		transform.position += transform.forward * 15 * Time.deltaTime;
	}

	void FixedUpdate()
	{
		// float moveSpeed = 35f;
		//  rigidbody.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.Acceleration);

	}
}

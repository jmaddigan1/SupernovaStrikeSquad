using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeEvent
{
	public string Name = "";
	public string Description = "";

	public EnvironmentData Environment;

	public virtual void OnStartEvent() { Debug.Log("OnStartEvent"); }

	public virtual void OnEndEvent() { Debug.Log("OnEndEvent"); }

	public virtual bool IsOver()
	{
		Debug.Log("IsOver Check");

		return true;
	}
}

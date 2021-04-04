using System.Collections;
using UnityEngine;

public abstract class MenuTransition : MonoBehaviour
{
	public abstract IEnumerator Play(Menu owner);
}

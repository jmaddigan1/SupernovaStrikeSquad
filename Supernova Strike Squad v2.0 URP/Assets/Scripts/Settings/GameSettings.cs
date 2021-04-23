using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	[Range(0, 1)] public float MasterVolume = 1;
	[Range(0, 1)] public float AmbientVolume = 0.3f;
	[Range(0, 1)] public float UIVolume = 0.2f;
}

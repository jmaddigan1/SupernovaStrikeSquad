using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	public int settingsValue1;
	public int settingsValue2;
	public int settingsValue3;

	public string settingsName1;
	public string settingsName2;
	public string settingsName3;
}

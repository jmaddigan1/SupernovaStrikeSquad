using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The IDs for each enemy type
/// </summary>
public enum EnemyType
{
	ExampleEnemyID1,
	ExampleEnemyID2,
	ExampleEnemyID3,
}

/// <summary>
/// The IDs for each weapon type
/// </summary>
public enum WeaponType
{
	Minigun,
	Rockets,
	Charge,
	Laser,
	Shotgun,
	Arc,
	Sniper,
	Drones,
	Teather,
}

/// <summary>
/// The Game Mode for our Node Map
/// </summary>
public enum GameModeType
{
	Campaign,
	MissionBoard,
	Endless
}

/// <summary>
/// The Lobby Type we are connected through
/// </summary>
public enum LobbyType
{
	Local,
	Steam
}

/// <summary>
/// The States of a Dock
/// </summary>
public enum DockState
{
	Open,
	Transitioning,
	Closed
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public enum EnemyType
{
	//
	Fighter,
	Charger,
	Juggernaut,

	//
	TestBoss
}

// 
public enum RewardType
{
	// If this a temporary reward like ammo or health.
	// A 'during encounter' pickup..
	Temp,

	// Or is this a permanent reward
	// You are given this after completing a encounter.
	Permanent
}

// The shape of the environment
public enum EnvironmentType
{
	Sphere,
	Square
}

//
public enum LobbyState
{
	//
	WaitingToReady,

	//
	ReadyToEnterGame,

	//
	PlayingTransition,

	//
	InGame
}

//
public enum MissionTypes
{
	//
	None,

	//
	Campaign,

	//
	MissionBoard,

	//
	Endless
}
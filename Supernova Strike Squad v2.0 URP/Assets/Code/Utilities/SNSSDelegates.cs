using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public delegate void OnEnemyDeath();

//
public delegate void OnEndEncounter(bool victory);

//
public delegate void OnHealthUpdate(float value, float maxValue);

//
public delegate void OnShieldUpdate(float value, float maxValue);

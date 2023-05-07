using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonsterStats
{
    public int currentHP;
    public int currentXP;
    public int maxHP;
    public int maxXP;
    public string name;

    public int attack;
    public int defense;
    public int specialDefense;
    public int specialAttack;

    public int currentHunger;
    public int currentFreshness;
    public int currentHappyness;
    public int currentInspiration;
    public int maxHunger;
    public int maxFreshness;
    public int maxHappyness;
    public int maxInspiration;

    public string mosterID;
    public int serialNumber;
}

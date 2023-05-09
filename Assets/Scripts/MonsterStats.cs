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
    public string defaultName;

    public int attack;
    public int defense;
    public int specialDefense;
    public int specialAttack;

    public float attackMod;
    public float defenseMod;
    public float specialAttackMod;
    public float specialDefenseMod;

    public int currentHunger;
    public int maxHunger;
    public int currentHungerTicks;
    public int maxHungerTicks;

    public float currentHappyness;
    public float maxHappyness;

    public int currentTalent;
    public int maxTalent;
    public int currentTalentTicks;
    public int maxTalentTicks;

    public float currentVirtue;
    public float maxVirtue;

    public string mosterID;
    public int serialNumber;
}

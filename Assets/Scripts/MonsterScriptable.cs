using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monsters", order = 1)]
public class MonsterScriptable : ScriptableObject
{
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

    public int maxHunger;
    public int maxHungerTicks;

    public int maxTalent;
    public int maxTalentTicks;

    public string mosterID;
}

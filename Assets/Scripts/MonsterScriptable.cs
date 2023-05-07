using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monsters", order = 1)]
public class MonsterScriptable : ScriptableObject
{
    public int attack;
    public int specialAttack;
    public int defense;
    public int specialDefense;
    public int maxHP;
    public int maxXP;
    public string defaultName;
}

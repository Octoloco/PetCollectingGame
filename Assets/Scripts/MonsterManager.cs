using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;

    private Monster selectedMonster = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool MonsterExists(int serialNumber)
    {
        for (int i = 0; i < transform.childCount;  i++)
        {
            if (transform.GetChild(i).GetComponent<Monster>().statSheet.serialNumber == serialNumber) 
            {
                return true;
            }
        }
        return false;
    }

    public void SelectMonster(Monster selectedMonster) 
    { 
        this.selectedMonster = selectedMonster;
    }

    public Monster ReturnSelectedMonster()
    {
        return selectedMonster;
    }

    public void KillMonster(int serialNumber)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Monster>().statSheet.serialNumber == serialNumber)
            {
                MonsterGenerator.Instance.DeleteMonster(transform.GetChild(i).GetComponent<Monster>().statSheet);
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void TickMonsters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Monster>().TickStats();
        }
    }
}

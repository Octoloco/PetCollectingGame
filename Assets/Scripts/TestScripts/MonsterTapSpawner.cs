using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTapSpawner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MonsterGenerator.Instance.GenerateMonster("0000");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            MonsterGenerator.Instance.LoadMonsters();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Grid.Instance.SaveMaps();
        }
    }
}

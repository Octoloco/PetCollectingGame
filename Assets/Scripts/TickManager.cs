using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    [SerializeField] float maxTickTimer;
    
    float tickTimer;

    private void Start()
    {
        tickTimer = maxTickTimer;
    }

    void Update()
    {
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0)
        {
            tickTimer = maxTickTimer;
            MonsterManager.instance.TickMonsters();
            
        }
    }
}

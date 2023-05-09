using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCarePanel : MonoBehaviour
{
    public static MonsterCarePanel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
}

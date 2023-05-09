using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public static MonsterGenerator Instance;

    public GameObject monsterPrefab;
    public Transform monsterContainer;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateMonster(string monsterID)
    {
        Monster monster = Instantiate(monsterPrefab, monsterContainer).GetComponent<Monster>();

        MonsterScriptable monsterScriptable = Resources.Load("MonsterScriptables/" + monsterID) as MonsterScriptable;
        int serialNumber = System.Guid.NewGuid().GetHashCode();
        monster.SetStats(monsterScriptable, serialNumber, monsterID);

        SaveMonster(monster.statSheet);
    }

    public void SaveMonster(MonsterStats statSheet)
    {
        string json = JsonUtility.ToJson(statSheet);

        string encryptedString = Encoder.StringCipher.Encrypt(json, Encoder.StringCipher.encoderPass);

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/Data/Mon/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Data/Mon/");
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Data/Mon/" + statSheet.serialNumber, encryptedString);
    }

    public void DeleteMonster(MonsterStats statSheet)
    {
        System.IO.File.Delete(Application.persistentDataPath + "/Data/Mon/" + statSheet.serialNumber);
    }

    public void LoadMonsters()
    {
        IEnumerable<string> monstersSaved = System.IO.Directory.EnumerateFiles(Application.persistentDataPath + "/Data/Mon/");

        foreach (string monsterSerialNumber in monstersSaved)
        {
            string hash = System.IO.File.ReadAllText(monsterSerialNumber);
            string json = Encoder.StringCipher.Decrypt(hash, Encoder.StringCipher.encoderPass);
            MonsterStats loadedMonster = JsonUtility.FromJson<MonsterStats>(json);
            if (!MonsterManager.instance.MonsterExists(loadedMonster.serialNumber))
            {
                Monster monster = Instantiate(monsterPrefab, monsterContainer).GetComponent<Monster>();
                monster.statSheet = loadedMonster;
            }
        }
    }
}

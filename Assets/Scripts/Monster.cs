using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Tweak Variables")]
    [SerializeField] private float distanceToTargetCheck;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime;
    [SerializeField] private float waitAfterActionTime;

    public Tile targetTile;
    public Tile currentTile;
    private int totalStates = 2;
    private int currentState;
    private bool canMove = false;
    private Tile initialTile;

    public MonsterStats statSheet;

    void Start()
    {
        initialTile = Grid.Instance.GetRandomAvailableTile();
        currentTile = initialTile;
        transform.position = initialTile.transform.position;
        StartCoroutine(NewState(true, 0));
    }

    void Update()
    {
        if (canMove)
        {
            Move();
        }
    }

    private enum Behaviors
    {
        wait,
        move
    }

    public void TickStats()
    {
        statSheet.currentHungerTicks--;
        statSheet.currentTalentTicks--;

        if (statSheet.currentTalentTicks <= 0)
        {
            statSheet.currentTalent--;
            statSheet.currentTalentTicks = statSheet.maxTalentTicks;
        }

        if (statSheet.currentHungerTicks <= 0)
        {
            statSheet.currentHunger--;
            statSheet.currentHungerTicks = statSheet.maxHungerTicks;
        }

        if (statSheet.currentHunger < 0)
        {
            if (MonsterManager.instance.ReturnSelectedMonster() == this)
            {
                MenuManager.Instance.HideMCPanel();
            }
            MonsterManager.instance.KillMonster(statSheet.serialNumber);
        }

        if(statSheet.currentTalent < 0)
        {
            statSheet.currentTalent = 0;
        }

        Debug.Log("MaxH " + statSheet.maxHappyness);
        statSheet.currentHappyness = (((float)statSheet.currentHunger / (float)statSheet.maxHunger) + ((float)statSheet.currentTalent / (float)statSheet.maxTalent)) / (float)statSheet.maxHappyness;

        MenuManager.Instance.UpdateMonsterDetails();

        UpdateCombatStats();
    }

    public void UpdateCombatStats()
    {
        statSheet.attack = statSheet.attack + Mathf.RoundToInt(statSheet.currentHappyness * statSheet.attackMod);
        statSheet.specialAttack = statSheet.specialAttack + Mathf.RoundToInt(statSheet.currentHappyness * statSheet.specialAttackMod);
        statSheet.defense = statSheet.defense + Mathf.RoundToInt(statSheet.currentHappyness * statSheet.defenseMod);
        statSheet.specialDefense = statSheet.specialDefense + Mathf.RoundToInt(statSheet.currentHappyness * statSheet.specialDefenseMod);
    }

    private void MonsterActions()
    {
        switch (currentState)
        {
            case (int)Behaviors.wait:
                Wait();
                break;
            case (int)Behaviors.move:
                SelectNewTargetTile();
                if (targetTile == null) 
                {
                    canMove = false;
                    StartCoroutine(NewState(false, waitAfterActionTime));
                }
                else
                {
                    canMove = true;
                    Move();
                }
                break;

        }
    }

    private void Move()
    {
        Vector3 targetPosition = targetTile.transform.position;
        if ((targetPosition - transform.position).magnitude > distanceToTargetCheck)
        {
            Vector3 direction = targetPosition - transform.position;
            direction = direction.normalized;
            transform.position = transform.position + (direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            canMove = false;
            transform.position = targetPosition;
            currentTile = targetTile;
            StartCoroutine(NewState(false, waitAfterActionTime));
        }
    }

    private void Wait()
    {
        StartCoroutine(NewState(true, waitTime));
    }

    private IEnumerator NewState(bool waited, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int state;

        if (waited)
        {
            state = 1;
        }
        else
        {
            int decition = Random.Range(0, 5);
            if (decition > 0)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
        }

        currentState = state;
        MonsterActions();
    }

    private void SelectNewTargetTile()
    {
        targetTile = currentTile.GetRandomNeighbour(null);

        if (targetTile == null)
        {
            Debug.LogWarning("Cannot move");
        }
        else
        {
            Grid.Instance.ForceFreeTile(currentTile);
            Grid.Instance.OccupyTileMove(targetTile);
        }
    }

    public void SetStats(MonsterScriptable monsterScriptable, int serialNumber, string monsterID)
    {
        statSheet.maxHP = monsterScriptable.maxHP;
        statSheet.maxXP = monsterScriptable.maxXP;
        statSheet.currentXP = 0;
        statSheet.currentHP = monsterScriptable.maxHP;
        statSheet.defaultName = monsterScriptable.defaultName;

        statSheet.attack = monsterScriptable.attack;
        statSheet.specialAttack = monsterScriptable.specialAttack;
        statSheet.defense = monsterScriptable.defense;
        statSheet.specialDefense = monsterScriptable.specialDefense;

        statSheet.currentHunger = monsterScriptable.maxHunger;
        statSheet.currentTalent = 0;
        statSheet.currentHungerTicks = monsterScriptable.maxHungerTicks;
        statSheet.currentTalentTicks = monsterScriptable.maxTalentTicks;

        statSheet.maxHunger = monsterScriptable.maxHunger;
        statSheet.maxTalent = monsterScriptable.maxTalent;
        statSheet.maxTalentTicks = monsterScriptable.maxTalentTicks;
        statSheet.maxHungerTicks = monsterScriptable.maxHungerTicks;

        statSheet.maxVirtue = 1;
        statSheet.currentVirtue = .5f;

        statSheet.maxHappyness = 2;
        statSheet.currentHappyness = ((statSheet.currentHunger / statSheet.maxHunger) + (statSheet.currentTalent / statSheet.maxTalent)) / statSheet.maxHappyness;

        statSheet.mosterID = monsterID;
        statSheet.serialNumber = serialNumber;

        UpdateCombatStats();
    }

    public void SetStats(MonsterScriptable monsterScriptable, int serialNumber, string monsterID, string monsterName)
    {
        statSheet.maxHP = monsterScriptable.maxHP;
        statSheet.maxXP = monsterScriptable.maxXP;
        statSheet.currentXP = 0;
        statSheet.currentHP = monsterScriptable.maxHP;
        statSheet.defaultName = monsterName;

        statSheet.attack = monsterScriptable.attack;
        statSheet.specialAttack = monsterScriptable.specialAttack;
        statSheet.defense = monsterScriptable.defense;
        statSheet.specialDefense = monsterScriptable.specialDefense;

        statSheet.currentHunger = monsterScriptable.maxHunger;
        statSheet.currentTalent = 0;
        statSheet.currentHungerTicks = monsterScriptable.maxHungerTicks;
        statSheet.currentTalentTicks = monsterScriptable.maxTalentTicks;

        statSheet.maxHunger = monsterScriptable.maxHunger;
        statSheet.maxTalent = monsterScriptable.maxTalent;
        statSheet.maxTalentTicks = monsterScriptable.maxTalentTicks;
        statSheet.maxHungerTicks = monsterScriptable.maxHungerTicks;

        statSheet.maxVirtue = 1;
        statSheet.currentVirtue = .5f;

        statSheet.maxHappyness = 2;
        statSheet.currentHappyness = (((float)statSheet.currentHunger / (float)statSheet.maxHunger) + ((float)statSheet.currentTalent / (float)statSheet.maxTalent)) / (float)statSheet.maxHappyness;

        statSheet.mosterID = monsterID;
        statSheet.serialNumber = serialNumber;
        UpdateCombatStats();
    }
}

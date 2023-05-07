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
        //SelectNewTargetTile();
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
        statSheet.name = monsterScriptable.defaultName;

        statSheet.attack = monsterScriptable.attack;
        statSheet.specialAttack = monsterScriptable.specialAttack;
        statSheet.defense = monsterScriptable.defense;
        statSheet.specialDefense = monsterScriptable.specialDefense;

        statSheet.currentHunger = 10;
        statSheet.currentFreshness = 10;
        statSheet.currentHappyness = 10;
        statSheet.currentInspiration = 10;

        statSheet.maxHunger = 10;
        statSheet.maxFreshness = 10;
        statSheet.maxHappyness = 10;
        statSheet.maxInspiration = 10;

        statSheet.mosterID = monsterID;
        statSheet.serialNumber = serialNumber;
    }
}

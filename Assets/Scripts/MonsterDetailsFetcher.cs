using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterDetailsFetcher : MonoBehaviour
{
    private TMP_Text text;
    private Monster selectedMonster = null;

    [SerializeField] private string statToFetch;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void FetchStat()
    {
        selectedMonster = MonsterManager.instance.ReturnSelectedMonster();

        if (statToFetch == "currentHP")
        {
            text.text = selectedMonster.statSheet.currentHP.ToString();
        }
        else if (statToFetch == "currentXP")
        {
            text.text = selectedMonster.statSheet.currentXP.ToString();
        }
        else if (statToFetch == "maxHP")
        {
            text.text = selectedMonster.statSheet.maxHP.ToString();
        }
        else if (statToFetch == "maxXP")
        {
            text.text = selectedMonster.statSheet.maxXP.ToString();
        }
        else if (statToFetch == "name")
        {
            text.text = selectedMonster.statSheet.name.ToString();
        }
        else if (statToFetch == "attack")
        {
            text.text = selectedMonster.statSheet.attack.ToString();
        }
        else if (statToFetch == "defense")
        {
            text.text = selectedMonster.statSheet.defense.ToString();
        }
        else if (statToFetch == "specialDefense")
        {
            text.text = selectedMonster.statSheet.specialDefense.ToString();
        }
        else if (statToFetch == "specialAttack")
        {
            text.text = selectedMonster.statSheet.specialAttack.ToString();
        }
        else if (statToFetch == "currentHunger")
        {
            text.text = selectedMonster.statSheet.currentHunger.ToString();
        }
        else if (statToFetch == "currentFreshness")
        {
            text.text = selectedMonster.statSheet.currentFreshness.ToString();
        }
        else if (statToFetch == "currentHappyness")
        {
            text.text = selectedMonster.statSheet.currentHappyness.ToString();
        }
        else if (statToFetch == "currentInspiration")
        {
            text.text = selectedMonster.statSheet.currentInspiration.ToString();
        }
        else if (statToFetch == "maxHunger")
        {
            text.text = selectedMonster.statSheet.maxHunger.ToString();
        }
        else if (statToFetch == "maxFreshness")
        {
            text.text = selectedMonster.statSheet.maxFreshness.ToString();
        }
        else if (statToFetch == "maxHappyness")
        {
            text.text = selectedMonster.statSheet.maxHappyness.ToString();
        }
        else if (statToFetch == "maxInspiration")
        {
            text.text = selectedMonster.statSheet.maxInspiration.ToString();
        }
        else if (statToFetch == "mosterID")
        {
            text.text = selectedMonster.statSheet.mosterID.ToString();
        }
        else if (statToFetch == "serialNumber")
        {
            text.text = selectedMonster.statSheet.serialNumber.ToString();
        }
    }
}

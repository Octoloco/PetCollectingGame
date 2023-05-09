using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterDetailsFetcher : MonoBehaviour
{
    private TMP_Text text;
    private Slider slider;
    private Monster selectedMonster = null;

    [SerializeField] private string statToFetch;
    [SerializeField] private bool isText;

    private void Start()
    {
        if (isText)
        {
            text = GetComponent<TMP_Text>();
        }
        else
        {
            slider = GetComponent<Slider>();
        }
    }

    public void FetchStat()
    {
        selectedMonster = MonsterManager.instance.ReturnSelectedMonster();

        if (selectedMonster != null)
        {
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
                text.text = selectedMonster.statSheet.defaultName;
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
            else if (statToFetch == "currentTalent")
            {
                text.text = selectedMonster.statSheet.currentTalent.ToString();
            }
            else if (statToFetch == "currentHappyness")
            {
                Debug.Log(selectedMonster.statSheet.currentHappyness);
                slider.value = selectedMonster.statSheet.currentHappyness;
            }
            else if (statToFetch == "currentVirtue")
            {
                slider.value = selectedMonster.statSheet.currentVirtue;
            }
            else if (statToFetch == "maxHunger")
            {
                text.text = selectedMonster.statSheet.maxHunger.ToString();
            }
            else if (statToFetch == "maxTalent")
            {
                text.text = selectedMonster.statSheet.maxTalent.ToString();
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
}

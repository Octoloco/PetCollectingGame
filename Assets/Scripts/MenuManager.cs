using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Animator MCPanelAnimator;
    [SerializeField] private Animator TilePanelAnimator;
    [SerializeField] private Animator ObjectPanelAnimator;

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

    public void UpdateMonsterDetails()
    {
        var detailsFetchers = GetComponentsInChildren<MonsterDetailsFetcher>();
        foreach (MonsterDetailsFetcher fetcher in detailsFetchers)
        {
            fetcher.FetchStat();
        }
    }

    public void ShowMCPanel()
    {
        MCPanelAnimator.SetBool("Show", true);
    }

    public void HideMCPanel()
    {
        MCPanelAnimator.SetBool("Show", false);
    }

    public void ShowTilePanel()
    {
        TilePanelAnimator.SetBool("Show", true);
    }

    public void HideTilePanel()
    {
        TilePanelAnimator.SetBool("Show", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void CloseBuildMode()
    {
        PlayerClickEvents.Instance.isBuiding = false;
        MenuManager.Instance.HideTilePanel();
    }

    public void OpenBuildMode()
    {
        PlayerClickEvents.Instance.isBuiding = true;
        MenuManager.Instance.ShowTilePanel();
    }

    public void SelectBuildTileID(int tileID)
    {
        PlayerClickEvents.Instance.selectedTileID = tileID;
    }

    public void SelectBuildTileFamilyID(int tileFamilyID)
    {
        PlayerClickEvents.Instance.selectedFamilyID = tileFamilyID;
    }

    public void SelectBuildTileIsPath(bool isPath)
    {
        PlayerClickEvents.Instance.selectedTileIsPath = isPath;
    }

    public void SelectBuildTileIsBlock(bool isBlock)
    {
        PlayerClickEvents.Instance.selectedTileIsBlock = isBlock;
    }

    public void SelectBuildTileIsObject(bool isObject)
    {
        PlayerClickEvents.Instance.selectedTileIsObject = isObject;
    }

    public void SelectBuildTileIsRemoveObject(bool isRemove)
    {
        PlayerClickEvents.Instance.selectedRemove = isRemove;
    }
}

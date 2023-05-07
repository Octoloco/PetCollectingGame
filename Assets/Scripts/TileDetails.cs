using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TileDetails
{
    public bool isOccupied;
    public int xPosition;
    public int yPosition;
    public int tileFamilyID;
    public int tileID;
    public bool isBlockTile;
    public bool isPathTile;
    public bool isGround;
}

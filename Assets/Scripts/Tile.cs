using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Tile : MonoBehaviour
{
    public TileDetails tileDetails;

    private Tile n = null;
    private Tile s = null;
    private Tile e = null;
    private Tile w = null;
    private Tile ne = null;
    private Tile nw = null;
    private Tile se = null;
    private Tile sw = null;
    private List<Tile> availableNeighbours = new List<Tile>();

    enum TileMatrix : short
    {
        n = 1 << 0,
        s = 1 << 1,
        e = 1 << 2,
        w = 1 << 3,
        ne = 1 << 4,
        nw = 1 << 5,
        se = 1 << 6,
        sw = 1 << 7,
        all = n|s|e|w|ne|nw|se|sw
    }

    public void CheckForPath()
    {
        foreach (Tile tile in availableNeighbours)
        {
            if (tile.tileDetails.isPathTile)
            {
                tile.PathChosen();
            }
        }
    }

    public void PathChosen()
    {

        TileMatrix matrix = 0;
        int pathTileID = 0;

        if (n != null)
        {
            if (n.tileDetails.isPathTile && n.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.n;
            }
        }
        if (s != null)
        {
            if (s.tileDetails.isPathTile && s.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.s;
            }
        }
        if (e != null)
        {
            if (e.tileDetails.isPathTile && e.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.e;
            }
        }
        if (w != null)
        {
            if (w.tileDetails.isPathTile && w.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.w;
            }
        }
        if (ne != null)
        {
            if (ne.tileDetails.isPathTile && ne.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.ne;
            }
        }
        if (nw != null)
        {
            if (nw.tileDetails.isPathTile && nw.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.nw;
            }
        }
        if (se != null)
        {
            if (se.tileDetails.isPathTile && se.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.se;
            }
        }
        if (sw != null)
        {
            if (sw.tileDetails.isPathTile && sw.tileDetails.tileFamilyID == tileDetails.tileFamilyID)
            {
                matrix |= TileMatrix.sw;
            }
        }

        if (matrix == TileMatrix.all)
        {
            pathTileID = 0;
        }
        else if ((matrix ^ TileMatrix.nw) == TileMatrix.all)
        {
            pathTileID = 1;
        }
        else if ((matrix ^ TileMatrix.ne) == TileMatrix.all)
        {
            pathTileID = 2;
        }
        else if ((matrix ^ TileMatrix.sw) == TileMatrix.all)
        {
            pathTileID = 3;
        }
        else if ((matrix ^ TileMatrix.se) == TileMatrix.all)
        {
            pathTileID = 4;
        }
        else if ((matrix ^ (TileMatrix.nw | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 5;
        }
        else if ((matrix ^ (TileMatrix.se | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 6;
        }
        else if ((matrix ^ (TileMatrix.nw | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 7;
        }
        else if ((matrix ^ (TileMatrix.se | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 8;
        }
        else if ((matrix ^ (TileMatrix.ne | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 9;
        }
        else if ((matrix ^ (TileMatrix.nw | TileMatrix.se)) == TileMatrix.all)
        {
            pathTileID = 10;
        }
        else if ((matrix ^ (TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 11;
        }
        else if ((matrix ^ (TileMatrix.ne | TileMatrix.se | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 12;
        }
        else if ((matrix ^ (TileMatrix.ne | TileMatrix.nw | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 13;
        }
        else if ((matrix ^ (TileMatrix.ne | TileMatrix.nw | TileMatrix.se)) == TileMatrix.all)
        {
            pathTileID = 14;
        }
        else if (((matrix | TileMatrix.ne | TileMatrix.nw) ^ (TileMatrix.n)) == TileMatrix.all)
        {
            pathTileID = 15;
        }
        else if (((matrix | TileMatrix.ne | TileMatrix.se) ^ (TileMatrix.e)) == TileMatrix.all)
        {
            pathTileID = 16;
        }
        else if (((matrix | TileMatrix.nw | TileMatrix.sw) ^ (TileMatrix.w)) == TileMatrix.all)
        {
            pathTileID = 17;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.sw) ^ (TileMatrix.s)) == TileMatrix.all)
        {
            pathTileID = 18;
        }
        else if ((matrix ^ (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 19;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.sw) ^ (TileMatrix.s | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 20;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.sw) ^ (TileMatrix.s | TileMatrix.nw)) == TileMatrix.all)
        {
            pathTileID = 21;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.ne) ^ (TileMatrix.e | TileMatrix.nw)) == TileMatrix.all)
        {
            pathTileID = 22;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.ne) ^ (TileMatrix.e | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 23;
        }
        else if (((matrix | TileMatrix.sw | TileMatrix.nw) ^ (TileMatrix.w | TileMatrix.se)) == TileMatrix.all)
        {
            pathTileID = 24;
        }
        else if (((matrix | TileMatrix.sw | TileMatrix.nw) ^ (TileMatrix.w | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 25;
        }
        else if (((matrix | TileMatrix.ne | TileMatrix.nw) ^ (TileMatrix.n | TileMatrix.se)) == TileMatrix.all)
        {
            pathTileID = 26;
        }
        else if (((matrix | TileMatrix.ne | TileMatrix.nw) ^ (TileMatrix.n | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 27;
        }
        else if (((matrix | TileMatrix.ne | TileMatrix.nw) ^ (TileMatrix.n | TileMatrix.sw | TileMatrix.se)) == TileMatrix.all)
        {
            pathTileID = 28;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.sw) ^ (TileMatrix.s | TileMatrix.nw | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 29;
        }
        else if (((matrix | TileMatrix.se | TileMatrix.ne) ^ (TileMatrix.e | TileMatrix.nw | TileMatrix.sw)) == TileMatrix.all)
        {
            pathTileID = 30;
        }
        else if (((matrix | TileMatrix.nw | TileMatrix.sw) ^ (TileMatrix.w | TileMatrix.se | TileMatrix.ne)) == TileMatrix.all)
        {
            pathTileID = 31;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.sw)) == (TileMatrix.e | TileMatrix.s | TileMatrix.se | TileMatrix.ne | TileMatrix.nw | TileMatrix.sw))
        {
            pathTileID = 32;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se)) == (TileMatrix.s | TileMatrix.w | TileMatrix.sw | TileMatrix.ne | TileMatrix.nw | TileMatrix.se))
        {
            pathTileID = 33;
        }
        else if ((matrix | (TileMatrix.se | TileMatrix.nw | TileMatrix.sw)) == (TileMatrix.n | TileMatrix.e | TileMatrix.ne | TileMatrix.se | TileMatrix.nw | TileMatrix.sw))
        {
            pathTileID = 34;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.sw | TileMatrix.se)) == (TileMatrix.w | TileMatrix.n | TileMatrix.nw | TileMatrix.ne | TileMatrix.sw | TileMatrix.se))
        {
            pathTileID = 35;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.sw)) == (TileMatrix.e | TileMatrix.s | TileMatrix.ne | TileMatrix.nw | TileMatrix.sw))
        {
            pathTileID = 36;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se)) == (TileMatrix.s | TileMatrix.w | TileMatrix.ne | TileMatrix.nw | TileMatrix.se))
        {
            pathTileID = 37;
        }
        else if ((matrix | (TileMatrix.se | TileMatrix.nw | TileMatrix.sw)) == (TileMatrix.n | TileMatrix.e | TileMatrix.se | TileMatrix.nw | TileMatrix.sw))
        {
            pathTileID = 38;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.sw | TileMatrix.se)) == (TileMatrix.w | TileMatrix.n | TileMatrix.ne | TileMatrix.sw | TileMatrix.se))
        {
            pathTileID = 39;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.n | TileMatrix.s))
        {
            pathTileID = 40;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.e | TileMatrix.w))
        {
            pathTileID = 41;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.n))
        {
            pathTileID = 42;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.s))
        {
            pathTileID = 43;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.w))
        {
            pathTileID = 44;
        }
        else if ((matrix | (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw)) == (TileMatrix.ne | TileMatrix.nw | TileMatrix.se | TileMatrix.sw | TileMatrix.e))
        {
            pathTileID = 45;
        }
        else 
        {
            pathTileID = 46;
        }

        if (pathTileID != tileDetails.tileID)
        {
            tileDetails.tileID = pathTileID;
            Sprite sprite = Grid.Instance.GetPathSprite(tileDetails);
            GetComponent<SpriteRenderer>().sprite = sprite;
            foreach(Tile tile in availableNeighbours)
            {
                if (tile.tileDetails.isPathTile)
                {
                    tile.PathChosen();
                }
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Grid.Instance.GetPathSprite(tileDetails);
        }
    }

    public Tile GetRandomNeighbour(List<Tile> availableNextNeighbours)
    {

        List<Tile> posibleDirections;
        
        if (availableNextNeighbours == null)
        {
            SetupNeighbours();
            posibleDirections = availableNeighbours;
        }
        else
        {
            posibleDirections = availableNextNeighbours;
        }
        
        Tile nextTile = this;
        if (posibleDirections.Count > 0)
        {
            int dir = Random.Range(0, posibleDirections.Count);
            nextTile = posibleDirections[dir];
            if (nextTile.tileDetails.isOccupied)
            {
                posibleDirections.Remove(nextTile);
                nextTile = GetRandomNeighbour(posibleDirections);
            }
            return nextTile;
        }

        return null;
    }

    public void SetupNeighbours()
    {
        n = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition, tileDetails.yPosition - 1);
        s = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition, tileDetails.yPosition + 1);
        e = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition + 1, tileDetails.yPosition);
        w = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition - 1, tileDetails.yPosition);
        ne = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition + 1, tileDetails.yPosition - 1);
        nw = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition - 1, tileDetails.yPosition - 1);
        se = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition + 1, tileDetails.yPosition + 1);
        sw = Grid.Instance.GetGroundTileByCoordinate(tileDetails.xPosition - 1, tileDetails.yPosition + 1);

        if (n != null)
        {
            availableNeighbours.Add(n);
        }

        if (s != null)
        {
            availableNeighbours.Add(s);
        }

        if (e != null)
        {
            availableNeighbours.Add(e);
        }

        if (w != null)
        {
            availableNeighbours.Add(w);
        }

        if (ne != null)
        {
            availableNeighbours.Add(ne);
        }

        if (nw != null)
        {
            availableNeighbours.Add(nw);
        }

        if (se != null)
        {
            availableNeighbours.Add(se);
        }

        if (sw != null)
        {
            availableNeighbours.Add(sw);
        }

        
    }

    public void ChangeSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Tile> posibleDirections;

    void Start()
    {

    }

    void Update()
    {

    }

    public Tile GetRandomNeighbour(List<Tile> availableNextNeighbours)
    {
        
        if (availableNextNeighbours == null)
        {
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

    public void ChangeSprite(Sprite sprite, bool occipied)
    {
        if (occipied)
        {
            Grid.Instance.OccupyTile(this);
        }
        else
        {
            Grid.Instance.FreeTile(this);
        }

        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

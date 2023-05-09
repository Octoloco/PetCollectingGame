using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    public static Grid Instance;

    [Header("Tiles Sprites")]
    [SerializeField] private GameObject tilePrefab;
    public int gridXSize;
    public int gridYSize;

    private List<List<Sprite>> randomTilesSprites = new List<List<Sprite>>();
    private List<List<Sprite>> blockTilesSprites = new List<List<Sprite>>();
    private List<List<Sprite>> pathTilesSprites = new List<List<Sprite>>();
    private List<List<Sprite>> pathBlockTilesSprites = new List<List<Sprite>>();
    private List<List<Sprite>> objectTilesSprites = new List<List<Sprite>>();
    private List<List<Sprite>> objectBlockTilesSprites = new List<List<Sprite>>();

    public List<Tile> groundTiles = new List<Tile>();
    public List<Tile> objectTiles = new List<Tile>();
    public List<Tile> availbaleTiles = new List<Tile>();

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

    void Start()
    {
        LoadGrid();
    }

    void Update()
    {

    }

    private void LoadTiles()
    {
        string[] tileFolders = Directory.GetDirectories(Application.dataPath + "/Resources/Tiles");
        Sprite[] spriteList = null;
        string[] folderDir = null;
        string[] tileFamilyFolders = null;
        string[] tileFamily = null;

        foreach (string folder in tileFolders)
        {
            folderDir = folder.Split("\\");
            tileFamilyFolders = Directory.GetDirectories(Application.dataPath + "/Resources/Tiles/" + folderDir[1]);
            foreach (string tileFamilyFolder in tileFamilyFolders)
            {
                List<Sprite> tempList = new List<Sprite>();
                tileFamily = tileFamilyFolder.Split("\\");
                spriteList = Resources.LoadAll<Sprite>("Tiles/" + folderDir[1] + "/" + tileFamily[1]);

                tempList.AddRange(spriteList);
                if (folderDir[1] == "Random")
                {
                    randomTilesSprites.Add(tempList);
                }
                else if (folderDir[1] == "Block")
                {
                    blockTilesSprites.Add(tempList);
                }
                else if (folderDir[1] == "Path")
                {
                    pathTilesSprites.Add(tempList);
                }
                else if (folderDir[1] == "PathBlock")
                {
                    pathBlockTilesSprites.Add(tempList);
                }
            }
        }


        tileFolders = Directory.GetDirectories(Application.dataPath + "/Resources/Objects");
        foreach (string folder in tileFolders)
        {
            folderDir = folder.Split("\\");
            tileFamilyFolders = Directory.GetDirectories(Application.dataPath + "/Resources/Objects/" + folderDir[1]);
            foreach (string tileFamilyFolder in tileFamilyFolders)
            {
                List<Sprite> tempList = new List<Sprite>();
                tileFamily = tileFamilyFolder.Split("\\");
                spriteList = Resources.LoadAll<Sprite>("Objects/" + folderDir[1] + "/" + tileFamily[1]);

                tempList.AddRange(spriteList);
                if (folderDir[1] == "Block")
                {
                    objectBlockTilesSprites.Add(tempList);
                }
                else if (folderDir[1] == "UnBlock")
                {
                    objectTilesSprites.Add(tempList);
                }
            }
        }
    }

    public void SetObjectTile(Tile tile, bool hasJson)
    {
        Sprite spriteToSet = null;

        if (!hasJson)
        {
            tile.tileDetails.tileFamilyID = 0;
            tile.tileDetails.tileID = 0;
            tile.tileDetails.isActive = false;
            tile.gameObject.SetActive(false);
        }
        else
        {
            if (!tile.tileDetails.isActive)
            {
                tile.gameObject.SetActive(false);
            }
            else
            {
                tile.gameObject.SetActive(true);
                if (!tile.tileDetails.isBlockTile)
                {
                    spriteToSet = objectTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
                else
                {
                    spriteToSet = objectBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
            }
        }

        tile.GetComponent<SpriteRenderer>().sprite = spriteToSet;
    }

    public void SetGroundTile(Tile tile, bool hasJson)
    {
        Sprite spriteToSet = null;

        if (!hasJson)
        {
            int randomSelectionIndex = Random.Range(0, randomTilesSprites[0].Count);
            spriteToSet = randomTilesSprites[0][randomSelectionIndex];
            tile.tileDetails.tileFamilyID = 0;
            tile.tileDetails.tileID = randomSelectionIndex;
        }
        else
        {
            if (tile.tileDetails.isPathTile)
            {
                if (tile.tileDetails.isBlockTile)
                {
                    spriteToSet = pathBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
                else
                {
                    spriteToSet = pathTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
            }
            else
            {
                if (tile.tileDetails.isBlockTile)
                {
                    spriteToSet = blockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
                else
                {
                    spriteToSet = randomTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                }
            }
        }

        tile.GetComponent<SpriteRenderer>().sprite = spriteToSet;
    }

    public int ChooseTileDirection()
    {

        return 0;
    }

    public void GenerateGroundGrid(string json)
    {
        for (int i = 0; i < gridXSize; i++)
        {
            for (int j = 0; j < gridYSize; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.GetComponent<Tile>().tileDetails.xPosition = i;
                newTile.GetComponent<Tile>().tileDetails.yPosition = j;
                newTile.GetComponent<Tile>().tileDetails.isGround = true;
                SetupTiles(newTile);
            }
        }

        if (json != null)
        {
            Maps loadedMaps = JsonUtility.FromJson<Maps>(json);
            for (int i = 0; i < groundTiles.Count; i++)
            {
                groundTiles[i].tileDetails = loadedMaps.groundTilesDetails[i];
                SetTilesSprites(groundTiles[i], true);
            }
        }
    }

    public void GenerateObjectGrid(string json)
    {
        for (int i = 0; i < gridXSize; i++)
        {
            for (int j = 0; j < gridYSize; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.GetComponent<Tile>().tileDetails.xPosition = i;
                newTile.GetComponent<Tile>().tileDetails.yPosition = j;
                newTile.GetComponent<Tile>().tileDetails.isGround = false;
                SetupTiles(newTile);
            }
        }

        if (json != null)
        {
            Maps loadedMaps = JsonUtility.FromJson<Maps>(json);
            for (int i = 0; i < groundTiles.Count; i++)
            {
                objectTiles[i].tileDetails = loadedMaps.objectTilesDetails[i];
                SetTilesSprites(objectTiles[i], true);
            }
        }
    }

    public void SetupTiles(GameObject newTile)
    {
        newTile.transform.position = new Vector3(newTile.GetComponent<Tile>().tileDetails.xPosition * tilePrefab.transform.localScale.x, -newTile.GetComponent<Tile>().tileDetails.yPosition * tilePrefab.transform.localScale.y, 0);

        if (newTile.GetComponent<Tile>().tileDetails.isGround)
        {
            newTile.GetComponent<SpriteRenderer>().sortingOrder = -2;
            groundTiles.Add(newTile.GetComponent<Tile>());

        }
        else
        {
            newTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
            objectTiles.Add(newTile.GetComponent<Tile>());
        }

        SetTilesSprites(newTile.GetComponent<Tile>(), false);
    }

    private void SetupTileNeighbours()
    {
        foreach (Tile groundTile in groundTiles)
        {
            groundTile.SetupNeighbours();
        }

        foreach (Tile groundTile in objectTiles)
        {
            groundTile.SetupNeighbours();
        }
    }

    public struct Maps
    {
        public TileDetails[] groundTilesDetails;
        public TileDetails[] objectTilesDetails;
    }

    public void SerializeMaps()
    {
        TileDetails[] groundTilesDetails;
        TileDetails[] objectTilesDetails;
        List<TileDetails> groundTilesDetailsList = new List<TileDetails>();
        List<TileDetails> objectTilesDetailsList = new List<TileDetails>();
        foreach (Tile tile in groundTiles)
        {
            groundTilesDetailsList.Add(tile.tileDetails);
        }

        foreach (Tile tile in objectTiles)
        {
            objectTilesDetailsList.Add(tile.tileDetails);
        }
        groundTilesDetails = groundTilesDetailsList.ToArray();
        objectTilesDetails = objectTilesDetailsList.ToArray();

        Maps maps;
        maps.groundTilesDetails = groundTilesDetails;
        maps.objectTilesDetails = objectTilesDetails;

        string mapJason = JsonUtility.ToJson(maps);
        string encryptedString = Encoder.StringCipher.Encrypt(mapJason, Encoder.StringCipher.encoderPass);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Data/Maps/Nursery", encryptedString);
    }

    public void SaveMaps()
    {
        TileDetails[] groundTilesDetails;
        TileDetails[] objectTilesDetails;
        List<TileDetails> groundTilesDetailsList = new List<TileDetails>();
        List<TileDetails> objectTilesDetailsList = new List<TileDetails>();
        foreach (Tile tile in groundTiles)
        {
            groundTilesDetailsList.Add(tile.tileDetails);
        }

        foreach (Tile tile in objectTiles)
        {
            objectTilesDetailsList.Add(tile.tileDetails);
        }
        groundTilesDetails = groundTilesDetailsList.ToArray();
        objectTilesDetails = objectTilesDetailsList.ToArray();

        Maps maps;
        maps.groundTilesDetails = groundTilesDetails;
        maps.objectTilesDetails = objectTilesDetails;

        string mapJason = JsonUtility.ToJson(maps);
        string encryptedString = Encoder.StringCipher.Encrypt(mapJason, Encoder.StringCipher.encoderPass);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Data/Maps/Nursery", encryptedString);
        Debug.Log("saved");
    }

    private void SetTilesSprites(Tile tile, bool hasJson)
    {
        if (tile.tileDetails.isGround)
        {
            SetGroundTile(tile, hasJson);
        }
        else
        {
            SetObjectTile(tile, hasJson);
        }
    }

    private void LoadGrid()
    {
        LoadTiles();

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/Data/Maps/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Data/Maps/");
            GenerateGroundGrid(null);
            GenerateObjectGrid(null);
            SetupTileNeighbours();

            SerializeMaps();
        }
        else
        {
            if (System.IO.Directory.GetFiles(Application.persistentDataPath + "/Data/Maps/").Length <= 0)
            {
                GenerateGroundGrid(null);
                GenerateObjectGrid(null);
                SetupTileNeighbours();

                SerializeMaps();
            }
            else
            {
                string[] mapSaved = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Data/Maps/");
                string hash = System.IO.File.ReadAllText(mapSaved[0]);
                string json = Encoder.StringCipher.Decrypt(hash, Encoder.StringCipher.encoderPass);
                GenerateGroundGrid(json);
                GenerateObjectGrid(json);
                SetupTileNeighbours();
            }
        }

        foreach (Tile tile in groundTiles)
        {
            if (!tile.tileDetails.isOccupied)
            {
                availbaleTiles.Add(tile);
            }
        }

        MonsterGenerator.Instance.LoadMonsters();
    }

    public Tile GetGroundTileByCoordinate(int x, int y)
    {
        foreach (Tile tile in groundTiles)
        {
            if (tile.tileDetails.xPosition == x && tile.tileDetails.yPosition == y)
            {
                return tile;
            }
        }
        return null;
    }

    public Tile GetObjectTileByCoordinate(int x, int y)
    {
        foreach (Tile tile in objectTiles)
        {
            if (tile.tileDetails.xPosition == x && tile.tileDetails.yPosition == y)
            {
                return tile;
            }
        }
        return null;
    }

    public Tile GetGroundTileByReference(Tile tileToLook)
    {
        foreach (Tile tile in groundTiles)
        {
            if (tileToLook == tile)
            {
                return tile;
            }
        }
        return null;
    }

    public Tile GetObjectTileByReference(Tile tileToLook)
    {
        foreach (Tile tile in objectTiles)
        {
            if (tileToLook == tile)
            {
                return tile;
            }
        }
        return null;
    }

    public void RemoveObject(Tile tile)
    {
        tile = GetObjectTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);

        if (tile.tileDetails.isActive)
        {
            tile.tileDetails.isActive = false;
            tile.tileDetails.isOccupied = false;
            tile.tileDetails.isBlockTile = false;
            tile.GetComponent<SpriteRenderer>().sprite = null;
            tile.gameObject.SetActive(false);
            tile = GetGroundTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
            FreeGroundTile(tile);
        }

        SaveMaps();
    }

    public Sprite GetPathSprite(TileDetails tileDetails)
    {
        Sprite selectedSprite;

        if (tileDetails.isBlockTile)
        {
            selectedSprite = pathBlockTilesSprites[tileDetails.tileFamilyID][tileDetails.tileID];
        }
        else
        {
            selectedSprite = pathTilesSprites[tileDetails.tileFamilyID][tileDetails.tileID];
        }

        return selectedSprite;
    }

    public void ReplaceTile(Tile tile, int newFamilyID, int newTileID, bool isBlock, bool isPath, bool isObject)
    {

        Sprite spriteToSet = null;

        

        if (!isObject)
        {
            tile = GetGroundTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
            tile.tileDetails.tileFamilyID = newFamilyID;
            tile.tileDetails.tileID = newTileID;

            if (isPath)
            {
                tile.tileDetails.isPathTile = true;

                if (isBlock)
                {
                    spriteToSet = pathBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    tile.tileDetails.isBlockTile = true;
                    OccupyGroundTile(tile);
                }
                else
                {
                    tile.tileDetails.isBlockTile = false;
                    FreeGroundTile(tile);
                    tile.PathChosen();
                    tile.CheckForPath();
                    SaveMaps();
                    return;
                }
            }
            else
            {
                tile.tileDetails.isPathTile = false;

                if (isBlock)
                {
                    spriteToSet = blockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    tile.tileDetails.isBlockTile = true;
                    OccupyGroundTile(tile);
                }
                else
                {
                    spriteToSet = randomTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    tile.tileDetails.isBlockTile = false;
                    tile.CheckForPath();
                    FreeGroundTile(tile);
                }
            }
        }
        else
        {
            tile = GetObjectTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
            tile.tileDetails.tileFamilyID = newFamilyID;
            tile.tileDetails.tileID = newTileID;
            tile.tileDetails.isActive = true;

            tile.gameObject.SetActive(true);

            if (isBlock)
            {
                spriteToSet = objectBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                tile.tileDetails.isBlockTile = true;
                OccupyObjectTile(tile);
                Tile groundTile = GetGroundTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
                OccupyGroundTile(groundTile);
            }
            else
            {
                spriteToSet = objectTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                tile.tileDetails.isBlockTile = false;
                FreeObjectTile(tile);
                Tile groundTile = GetGroundTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
                FreeGroundTile(groundTile);
            }
        }

        tile.GetComponent<SpriteRenderer>().sprite = spriteToSet;
        SaveMaps();
    }

    public Tile GetRandomAvailableTile()
    {
        int index = Random.Range(0, availbaleTiles.Count);
        return availbaleTiles[index];
    }

    public void OccupyGroundTile(Tile targetTile)
    {
        targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;
        availbaleTiles.Remove(targetTile);

    }

    public void OccupyObjectTile(Tile targetTile)
    {
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;

    }

    public void OccupyTileMove(Tile targetTile)
    {

        targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;
        availbaleTiles.Remove(targetTile);
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;

    }

    public void FreeGroundTile(Tile targetTile)
    {
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        if (!targetTile.tileDetails.isBlockTile)
        {
            targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
            if (!targetTile.tileDetails.isBlockTile)
            {
                targetTile.tileDetails.isOccupied = false;
                availbaleTiles.Add(targetTile);
            }
            else
            {
                targetTile.tileDetails.isOccupied = true;
            }
        }
    }

    public void FreeObjectTile(Tile targetTile)
    {
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = false;
    }

    public void ForceFreeTile(Tile targetTile)
    {
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = false;
        targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = false;
        availbaleTiles.Add(targetTile);
    }
}

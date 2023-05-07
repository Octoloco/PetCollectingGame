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
    private List<Sprite> groundTilesSprites = new List<Sprite>(); //remove

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
    }

    public void SetObjectTile(Tile tile, bool hasJson)
    {
        Sprite spriteToSet = null;

        if (!hasJson)
        {
            tile.tileDetails.tileFamilyID = 0;
            tile.tileDetails.tileID = 0;
            tile.gameObject.SetActive(false);
        }
        else
        {
            if (tile.tileDetails.tileFamilyID <= 0)
            {
                tile.gameObject.SetActive(false);
            }
            else
            {
                if (tile.tileDetails.isBlockTile)
                {
                    spriteToSet = objectTilesSprites[tile.tileDetails.tileFamilyID - 1][tile.tileDetails.tileID];
                }
                else
                {
                    spriteToSet = objectBlockTilesSprites[tile.tileDetails.tileFamilyID - 1][tile.tileDetails.tileID];
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
                    spriteToSet = pathBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
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

    public void GenerateGrid(bool isGroundGrid, string json)
    {
        for (int i = 0; i < gridXSize; i++)
        {
            for (int j = 0; j < gridYSize; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.GetComponent<Tile>().tileDetails.xPosition = i;
                newTile.GetComponent<Tile>().tileDetails.yPosition = j;
                newTile.GetComponent<Tile>().tileDetails.isGround = isGroundGrid;
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

    private void SetupTileLists()
    {
        //for (int i = 0; i < objectTiles.Count; i++)
        //{
        //    if (objectTiles[i].tileDetails.isOccupied)
        //    {
        //        groundTiles[i].tileDetails.isOccupied = true;
        //    }
        //    else
        //    {
        //        groundTiles[i].tileDetails.isOccupied = false;
        //        availbaleTiles.Add(groundTiles[i]);
        //    }
        //}

        foreach (Tile groundTile in groundTiles)
        {
            groundTile.SetupNeighbours();
        }
    }

    public struct Maps
    {
        public TileDetails[] groundTilesDetails;
    }

    public void SerializeMaps()
    {
        TileDetails[] groundTilesDetails;
        List<TileDetails> groundTilesDetailsList = new List<TileDetails>();
        foreach (Tile tile in groundTiles)
        {
            groundTilesDetailsList.Add(tile.tileDetails);
        }
        groundTilesDetails = groundTilesDetailsList.ToArray();

        Maps maps;
        maps.groundTilesDetails = groundTilesDetails;

        string mapJason = JsonUtility.ToJson(maps);
        string encryptedString = Encoder.StringCipher.Encrypt(mapJason, Encoder.StringCipher.encoderPass);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Data/Maps/Nursery", encryptedString);
    }

    public void SaveMaps()
    {
        TileDetails[] groundTilesDetails;
        List<TileDetails> groundTilesDetailsList = new List<TileDetails>();
        foreach (Tile tile in groundTiles)
        {
            groundTilesDetailsList.Add(tile.tileDetails);
        }
        groundTilesDetails = groundTilesDetailsList.ToArray();

        Maps maps;
        maps.groundTilesDetails = groundTilesDetails;

        string mapJason = JsonUtility.ToJson(maps);
        string encryptedString = Encoder.StringCipher.Encrypt(mapJason, Encoder.StringCipher.encoderPass);
        System.IO.File.Delete(Application.persistentDataPath + "/Data/Maps/Nursery");
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



        //foreach (Tile tile in groundTiles)
        //{
        //    Sprite tileSprite = groundTilesSprites[tile.tileDetails.tileID];
        //    bool occupancy;
        //    if (tile.tileDetails.tileID == 1)
        //    {
        //        occupancy = true;
        //    }
        //    else
        //    {
        //        occupancy = false;
        //    }

        //    tile.ChangeSprite(tileSprite, occupancy);
        //}

        //foreach (Tile tile in objectTiles)
        //{
        //    if (tile.tileDetails.tileID == groundTilesSprites.Count)
        //    {
        //        tile.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        Sprite tileSprite = objectTilesSprites[tile.tileDetails.tileID - groundTilesSprites.Count - 1];
        //        tile.ChangeSprite(tileSprite, true);
        //    }
        //}
    }

    private void LoadGrid()
    {
        LoadTiles();

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/Data/Maps/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Data/Maps/");
            GenerateGrid(true, null);
            GenerateGrid(false, null);
            SetupTileLists();

            SerializeMaps();
        }
        else
        {
            if (System.IO.Directory.GetFiles(Application.persistentDataPath + "/Data/Maps/").Length <= 0)
            {
                GenerateGrid(true, null);
                GenerateGrid(false, null);
                SetupTileLists();

                SerializeMaps();
            }
            else
            {
                string[] mapSaved = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Data/Maps/");
                string hash = System.IO.File.ReadAllText(mapSaved[0]);
                string json = Encoder.StringCipher.Decrypt(hash, Encoder.StringCipher.encoderPass);
                GenerateGrid(true, json);
                GenerateGrid(false, json);
                SetupTileLists();
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

    public void ReplaceTile(Tile tile, int newFamilyID, int newTileID, bool isBlock, bool isPath)
    {

        Sprite spriteToSet = null;

        tile.tileDetails.tileFamilyID = newFamilyID;
        tile.tileDetails.tileID = newTileID;

        if (tile.tileDetails.isGround)
        {
            if (isPath)
            {
                if (isBlock)
                {
                    spriteToSet = pathBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    OccupyTile(tile);
                }
                else
                {
                    spriteToSet = pathBlockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    FreeTile(tile);
                }
            }
            else
            {
                if (isBlock)
                {
                    Debug.Log(blockTilesSprites[tile.tileDetails.tileFamilyID].Count);
                    spriteToSet = blockTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    OccupyTile(tile);
                }
                else
                {
                    spriteToSet = randomTilesSprites[tile.tileDetails.tileFamilyID][tile.tileDetails.tileID];
                    FreeTile(tile);
                }
            }
        }
        else
        {

        }

        tile.GetComponent<SpriteRenderer>().sprite = spriteToSet;


        //else if (newID > groundTilesSprites.Count)
        //{
        //    tile = GetObjectTileByCoordinate(tile.tileDetails.xPosition, tile.tileDetails.yPosition);
        //    tile.gameObject.SetActive(true);
        //    tile.ChangeSprite(objectTilesSprites[newID - groundTilesSprites.Count - 1], true);
        //    tile.tileDetails.tileID = newID;
        //}

    }

    public Tile GetRandomAvailableTile()
    {
        int index = Random.Range(0, availbaleTiles.Count);
        return availbaleTiles[index];
    }

    public void OccupyTile(Tile targetTile)
    {

        targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;
        targetTile.tileDetails.isBlockTile = true;
        availbaleTiles.Remove(targetTile);
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;
        targetTile.tileDetails.isBlockTile = true;

    }

    public void OccupyTileMove(Tile targetTile)
    {

        targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;
        availbaleTiles.Remove(targetTile);
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        targetTile.tileDetails.isOccupied = true;

    }

    public void FreeTile(Tile targetTile)
    {
        targetTile = GetObjectTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
        if (!targetTile.tileDetails.isOccupied)
        {
            targetTile.tileDetails.isOccupied = false;
            targetTile.tileDetails.isBlockTile = false;
            targetTile = GetGroundTileByCoordinate(targetTile.tileDetails.xPosition, targetTile.tileDetails.yPosition);
            targetTile.tileDetails.isOccupied = false;
            targetTile.tileDetails.isBlockTile = false;
            availbaleTiles.Add(targetTile);
        }
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

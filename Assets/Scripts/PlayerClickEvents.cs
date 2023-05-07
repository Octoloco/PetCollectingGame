using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickEvents : MonoBehaviour
{
    public static PlayerClickEvents Instance;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomLerpInSpeed;
    [SerializeField] private float moveLerpSpeed;
    [SerializeField] private float zoomToMoveMod;
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamPosition;

    private bool drag;
    private bool lerpIn;
    private bool lerpOut;
    private bool lerpReset;
    private float zoomTotal;

    public int selectedFamilyID = 0;
    public int selectedTileID = 0;
    public bool selectedTileIsBlock = false;
    public bool selectedTileIsPath = false;
    public bool isBuiding = false;

    private GameObject hitLast = null;
    private GameObject hitLastGround = null;

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

    private void Start()
    {
        resetCamPosition = Camera.main.transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lerpReset = true;
            StartCoroutine(WaitForLerpReset());
        }

        if (isBuiding)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.transform.CompareTag("Tile"))
                    {
                        if (hitLast != Grid.Instance.GetObjectTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition).gameObject || hitLastGround != Grid.Instance.GetGroundTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition).gameObject)
                        {
                            if (hitLast != null)
                            {
                                Tile tile = Grid.Instance.GetObjectTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition);
                                tile.GetComponent<SpriteRenderer>().color = new Color(tile.transform.GetComponent<SpriteRenderer>().color.r, tile.transform.GetComponent<SpriteRenderer>().color.g, tile.transform.GetComponent<SpriteRenderer>().color.b, .75f);

                                tile = Grid.Instance.GetGroundTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition);
                                tile.GetComponent<SpriteRenderer>().color = new Color(tile.transform.GetComponent<SpriteRenderer>().color.r, tile.transform.GetComponent<SpriteRenderer>().color.g, tile.transform.GetComponent<SpriteRenderer>().color.b, .75f);

                                hitLast.GetComponent<SpriteRenderer>().color = new Color(hitLast.transform.GetComponent<SpriteRenderer>().color.r, hitLast.transform.GetComponent<SpriteRenderer>().color.g, hitLast.transform.GetComponent<SpriteRenderer>().color.b, 1);
                                hitLastGround.GetComponent<SpriteRenderer>().color = new Color(hitLastGround.transform.GetComponent<SpriteRenderer>().color.r, hitLastGround.transform.GetComponent<SpriteRenderer>().color.g, hitLastGround.transform.GetComponent<SpriteRenderer>().color.b, 1);

                            }
                            else
                            {
                                Tile tile = Grid.Instance.GetObjectTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition);
                                tile.GetComponent<SpriteRenderer>().color = new Color(tile.transform.GetComponent<SpriteRenderer>().color.r, tile.transform.GetComponent<SpriteRenderer>().color.g, tile.transform.GetComponent<SpriteRenderer>().color.b, .75f);

                                tile = Grid.Instance.GetGroundTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition);
                                tile.GetComponent<SpriteRenderer>().color = new Color(tile.transform.GetComponent<SpriteRenderer>().color.r, tile.transform.GetComponent<SpriteRenderer>().color.g, tile.transform.GetComponent<SpriteRenderer>().color.b, .75f);
                            }

                            hitLastGround = Grid.Instance.GetGroundTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition).gameObject;
                            hitLast = Grid.Instance.GetObjectTileByCoordinate(hit.transform.GetComponent<Tile>().tileDetails.xPosition, hit.transform.GetComponent<Tile>().tileDetails.yPosition).gameObject;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            Grid.Instance.ReplaceTile(hit.transform.GetComponent<Tile>(), selectedFamilyID, selectedTileID, selectedTileIsBlock, selectedTileIsPath);
                        }
                    }
                }
            }
            else
            {
                if (hitLast != null)
                {
                    hitLast.GetComponent<SpriteRenderer>().color = new Color(hitLast.transform.GetComponent<SpriteRenderer>().color.r, hitLast.transform.GetComponent<SpriteRenderer>().color.g, hitLast.transform.GetComponent<SpriteRenderer>().color.b, 1);
                    hitLastGround.GetComponent<SpriteRenderer>().color = new Color(hitLastGround.transform.GetComponent<SpriteRenderer>().color.r, hitLastGround.transform.GetComponent<SpriteRenderer>().color.g, hitLastGround.transform.GetComponent<SpriteRenderer>().color.b, 1);
                }
            }

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("Monster"))
                {
                    MonsterManager.instance.SelectMonster(hit.collider.gameObject.GetComponent<Monster>());
                    MenuManager.Instance.UpdateMonsterDetails();
                    MenuManager.Instance.ShowMCPanel();
                }
                else
                {
                    MenuManager.Instance.HideMCPanel();
                }
            }
            else
            {
                MenuManager.Instance.HideMCPanel();
            }
        }
        
        if (Input.GetMouseButton(1))
        {
            MenuManager.Instance.HideMCPanel();

            difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (!drag)
            {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
            if ((Camera.main.transform.position - resetCamPosition).magnitude > 3 + (((5 / Camera.main.orthographicSize) - 1) * zoomToMoveMod))
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, resetCamPosition, Time.deltaTime / moveLerpSpeed);
            }
        }

        if (drag)
        {
            Camera.main.transform.position = origin - difference;
        }

        if (Camera.main.orthographicSize <= 5 && Camera.main.orthographicSize >= 2)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;
            zoomTotal += Input.mouseScrollDelta.y;
            lerpIn = false;
            lerpOut = false;
        }
        else if (Camera.main.orthographicSize > 5)
        {
            StartCoroutine(WaitForZoomLerpIn());
        }
        else
        {
            StartCoroutine(WaitForZoomLerpOut());
        }

        if (lerpIn)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 4.9f, Time.deltaTime / zoomLerpInSpeed);
        }

        if (lerpOut)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2.1f, Time.deltaTime / zoomLerpInSpeed);
        }

        if (lerpReset)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5.1f, Time.deltaTime / zoomLerpInSpeed);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, resetCamPosition, Time.deltaTime / moveLerpSpeed);
        }
    }

    IEnumerator WaitForZoomLerpIn()
    {
        yield return new WaitForSeconds(0.2f);
        lerpIn = true;
    }

    IEnumerator WaitForZoomLerpOut()
    {
        yield return new WaitForSeconds(0.2f);
        lerpOut = true;
    }

    IEnumerator WaitForLerpReset()
    {
        yield return new WaitForSeconds(1f);
        lerpReset = false;
    }
}

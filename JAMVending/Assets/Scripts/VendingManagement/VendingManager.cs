using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class VendingManager : MonoBehaviour
{
    [SerializeField] private VendingObject vendingPrefab;
    [SerializeField] private GameObject grid;
    [SerializeField] private CustomCursor customCursor;

    public Sprite[] vendingSprites;
    public List<Tile> tiles;

    // Manages all the vending machine objects
    private List<VendingObject> vendingObjects;
    private Decoration decorationToPlace;
    private InputAction primaryClick;

    public static VendingManager instance { get; private set; }

    private void Awake()
    {
        tiles = new List<Tile>();
        vendingObjects = new List<VendingObject>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        customCursor.gameObject.SetActive(true);
        Cursor.visible = false;
    }

    private void Start()
    {

        primaryClick = GameManager.instance.InputAction.FindAction("UI/Click");
        primaryClick.Enable();

        primaryClick.performed += placeDecoration;
    }

    private void Update()
    {
    }

    private void placeDecoration(InputAction.CallbackContext context)
    {
        grid.SetActive(true);
        if (decorationToPlace != null)
        {
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (Tile tile in tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearestTile = tile;
                }
            }
            if (nearestTile != null && !nearestTile.isOccupied)
            {
                Instantiate(decorationToPlace, nearestTile.transform.position, Quaternion.identity);
                decorationToPlace = null;
                nearestTile.isOccupied = true;
                grid.SetActive(false);
                //customCursor.gameObject.SetActive(false);
                //Cursor.visible = true;
            }
        }
    }

    public void checkDecoration(Decoration deco)
    {
        decorationToPlace = deco;
        grid.SetActive(true);
        customCursor.gameObject.SetActive(true);
        customCursor.GetComponent<SpriteRenderer>().sprite = deco.GetComponent<SpriteRenderer>().sprite;
        Cursor.visible = false;
    }

    public void createVendingMachine(vendingType _type, string _name, Vector2 position)
    {
        VendingObject vending = Instantiate(vendingPrefab);
        vending.initialize(_type, _name, position);
        vendingObjects.Add(vending);
    }
}

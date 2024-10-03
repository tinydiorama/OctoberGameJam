using System.Collections.Generic;
using UnityEngine;

public class VendingManager : MonoBehaviour
{
    [SerializeField] private VendingObject vendingPrefab;
    public Sprite[] vendingSprites;

    // Manages all the vending machine objects
    private List<VendingObject> vendingObjects;

    public static VendingManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        vendingObjects = new List<VendingObject>();
    }

    public void createVendingMachine(vendingType _type, string _name, Vector2 position)
    {
        VendingObject vending = Instantiate(vendingPrefab);
        vending.initialize(_type, _name, position);
        vendingObjects.Add(vending);
    }
}

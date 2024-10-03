using UnityEngine;

public enum vendingType
{
    Drinks = 0,
    Snacks = 1,
    IceCream = 2
};

public class VendingObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteObj;

    private vendingType type;
    private string vendingName;
    private Vector2 position;

    public void initialize(vendingType _type, string _name, Vector2 position)
    {
        type = _type;
        vendingName = _name;
        this.position = position;

        spriteObj.sprite = VendingManager.instance.vendingSprites[(int)type];
    }
}

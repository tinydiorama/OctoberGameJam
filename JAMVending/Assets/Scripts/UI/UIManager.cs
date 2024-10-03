using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void addVending()
    {
        VendingManager.instance.createVendingMachine(vendingType.Snacks, "Test", new Vector2(0, 0));
    }
}

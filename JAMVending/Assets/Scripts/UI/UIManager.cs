using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void addVending()
    {
        VendingManager.instance.createVendingMachine(vendingType.Snacks, "Test", new Vector2(0, 0));
    }

    public void buyDecoration(Decoration obj)
    {
        if (GameManager.instance.Money >= obj.cost)
        {
            GameManager.instance.Money -= obj.cost;
            VendingManager.instance.checkDecoration(obj);
        }
    }

    public void updateMoney(int amount)
    {
        Debug.Log(amount);
    }
}

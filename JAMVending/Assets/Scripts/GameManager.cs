using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    public InputActionAsset InputAction;

    private int money;
    public static GameManager instance { get; private set; }
    public int Money
    {
        get
        {
            return this.money;
        }
        set
        {
            this.money = value;
            uiManager.updateMoney(this.money);
        }
    }

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

        Money = 50;
    }
}

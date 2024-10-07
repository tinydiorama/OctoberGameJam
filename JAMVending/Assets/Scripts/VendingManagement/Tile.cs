using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied;
    public Color occupiedColor;
    public Color vacantColor;

    private SpriteRenderer sr;

    private void Start()
    {
        VendingManager.instance.tiles.Add(this);
        sr = GetComponent<SpriteRenderer>();
        occupiedColor = Color.red;
        vacantColor = Color.green;
    }
    private void Update()
    {
        if (isOccupied)
        {
            sr.color = occupiedColor;
        }
        else
        {
            sr.color = vacantColor;
        }
    }
}

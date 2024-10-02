using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int m_width;
    private int m_height;
    private Vector3 m_originPosition;
    private float m_cellSize;
    private int[,] m_gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;
        m_gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        m_originPosition = originPosition;
        Debug.Log(width + " " + height);


        for (int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(m_gridArray[x, y].ToString(), Color.black, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        SetValue(1, 1, 4);

    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_cellSize + m_originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - m_originPosition).x / m_cellSize);
        y = Mathf.FloorToInt((worldPosition -m_originPosition).y / m_cellSize);

    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            m_gridArray[x, y] = value;
            debugTextArray[x, y].text = m_gridArray[x, y].ToString();

        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
            return m_gridArray[x, y];
        else
            return -1;

    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    #region DebugUtils
    public static TextMesh CreateWorldText(string text, Color color, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 1)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject go = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = go.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.alignment = textAlignment;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    #endregion
}

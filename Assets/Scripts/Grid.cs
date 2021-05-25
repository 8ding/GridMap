using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Grid
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged; 
    public class OnGridValueChangedEventArgs:EventArgs
    {
        public int x;
        public int y;
    }
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    private int height;
    private int width;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private float cellSize;
    private Vector3 originPosition;
    
    public Grid(int _width, int _height, float _cellSize,Vector3 originPosition)
    {
        this.height = _height;
        this.width = _width;
        this.cellSize = _cellSize;
        this.originPosition = originPosition;
        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        DrawGrid();
    }

    public void DrawGrid()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                debugTextArray[i, j] = CreateWorldText(null, gridArray[i, j].ToString(),
                    GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white,
                    TextAnchor.MiddleCenter, TextAlignment.Center, 1);

                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.red, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), Color.red, 100f);
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize + originPosition;
    }

    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void setValue(int x, int y, int value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = Mathf.Clamp(value,HEAT_MAP_MIN_VALUE,HEAT_MAP_MAX_VALUE);
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs {x = x, y = y});
        }
    }

    public void setValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        setValue(x,y,value);
    }
    TextMesh CreateWorldText(Transform parent,string text, Vector3 localPosition, int fontsize, Color color,
        TextAnchor textAnchor, TextAlignment textAlignment,int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.alignment = textAlignment;
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontsize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return gridArray[x, y];
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
}

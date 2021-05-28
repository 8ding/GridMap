using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MyGrid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; 
    public class OnGridObjectChangedEventArgs:EventArgs
    {
        public int x;
        public int y;
    }
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    private int height;
    private int width;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private float cellSize;
    private Vector3 originPosition;
    private bool isDebug = true;
    
    public MyGrid(int _width, int _height, float _cellSize,Vector3 originPosition,Func<MyGrid<TGridObject>,int,int,TGridObject> createGridObject)
    {
        this.height = _height;
        this.width = _width;
        this.cellSize = _cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = createGridObject(this, i, j);
            }
        }

        if (isDebug)
        {
            DrawGrid();
        }
    }

    public void OnTriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs{x = x, y = y});
        }
    }
    public void DrawGrid()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                debugTextArray[i, j] = CreateWorldText(null, gridArray[i, j]?.ToString(),
                    GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white,
                    TextAnchor.MiddleCenter, TextAlignment.Center, 1);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.red, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), Color.red, 100f);
        //自己给自己注册事件
        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs args) =>
        {
            debugTextArray[args.x, args.y].text = gridArray[args.x, args.y]?.ToString();
        };
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs {x = x, y = y});
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x,y,value);
    }
    //生成TextMesh用于在屏幕上显示grid的value,是用于debug网格的值的
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

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return gridArray[x, y];
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public void SetWalkable(bool walkable)
    {
        
    }

    // public void AddValue(Vector3 worldPosition,int value)
    // {
    //     setValue(worldPosition,GetValue(worldPosition)+value);
    // }
    // public void AddValue(int x, int y, int value)
    // {
    //     setValue(x, y, GetValue(x, y) + value);
    // }
    // /*点击鼠标后会调用该函数，函数内对grid的value进行修改，而每次修改value都会引发updateHeatMapVisual也就是网格更新，
    //  这样一帧之内要调用非常多次网格更新，因为一帧之内可以循环多次，所以每一帧只执行一次网格更新在lateupdate中进行*/
    //
    // public void AddValueRange(Vector3 position, int value, int fullValueRange, int totalRange)
    // {
    //     int lowerValueAmount = Mathf.RoundToInt((float) value / (totalRange - fullValueRange));
    //     
    //     GetXY(position, out int orginX, out int originY);
    //
    //     for (int x = 0; x < totalRange; x++)
    //     {
    //         for (int y = 0; y < totalRange - x; y++)
    //         {
    //             int radius = x + y;
    //             int addValueAmount = value;
    //             if (radius > fullValueRange)
    //             {
    //                 addValueAmount -= lowerValueAmount * (radius - fullValueRange);
    //             }
    //             AddValue(orginX + x, originY + y, addValueAmount);
    //             if (x != 0)
    //             {
    //                 AddValue(orginX - x, originY + y, addValueAmount);
    //             }
    //     
    //             if (y != 0)
    //     
    //             {
    //                 AddValue(orginX + x, originY - y, addValueAmount);
    //                 if (x != 0)
    //                 {
    //                     AddValue(orginX - x, originY - y, addValueAmount);
    //                 }
    //             }
    //         }
    //     }
    // }

    public float GetCellSize()
    {
        return cellSize;
    }
}

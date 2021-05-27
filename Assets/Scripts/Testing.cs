using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    // private MyGrid<HeatMapGridObject> grid;
    // [SerializeField] private HeatMapVisual heatMapVisual;
    // [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    // [SerializeField] private Vector3 originPosition;

    
    void Start()
    {
        //createGridObject是一个有返回值的事件，这个事件是构造函数的一个参数，在构造函数中可以利用这个参数进行一些操作，
        //但是在构造函数中只负责传参数，而在这个具体应用的地方则将泛型实例化，并决定这个事件注册了什么函数。
        //而在那边的构造函数中直接触发事件就调用了这个函数，也就调用了泛型的实例，在这里就是返回了HeatMapGridObject这个实例构造后的结果
        //grid = new MyGrid<HeatMapGridObject>(100, 100, 5f, new Vector3(0, 0),
            // ((MyGrid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y)));
        // heatMapVisual.SetGrid(grid);
        //heatMapGenericVisual.SetGrid(grid);
        Pathfinding pathfinding = new Pathfinding(10, 10);
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
    //             -1 * Camera.main.transform.position.z));
    //         // grid.setValue(mousePosition,new HeatMapGridObject(5));
    //         HeatMapGridObject gridObject = grid.GetGridObject(mousePosition);
    //         if (gridObject != null)
    //         {
    //             gridObject.AddValue(5);
    //         }
    //     }
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
    //             -1 * Camera.main.transform.position.z));
    //     }
    // }

}

public class HeatMapGridObject
{
    private int value;
    private const int MIN = 0;
    private const int MAX = 100;
    private int x;
    private int y;
    private MyGrid<HeatMapGridObject> grid;
    
    public HeatMapGridObject(MyGrid<HeatMapGridObject> grid,int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.OnTriggerGridObjectChanged(x,y);
    }

    public float GetValueNormalized()
    {
        return (float) value / MAX;
    }
    public override string ToString()
    {
        return value.ToString();
    }
}

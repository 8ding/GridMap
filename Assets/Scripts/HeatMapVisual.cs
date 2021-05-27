using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private MyGrid<HeatMapGridObject> grid;
    private Mesh mesh;
    private bool meshUpdate;
    public void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(MyGrid<HeatMapGridObject> grid)
    {
        this.grid = grid;

        UpDateHeatMapVisual();
        //grid的值发生变化会导致mesh发生变化
        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }
    

    public void UpDateHeatMapVisual()
    {
        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out vertices, out uv, out triangles);
        
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
                HeatMapGridObject gridValue = grid.GetGridObject(x, y);
                //归一化，当前value所占的比例值
                float gridValueNormalized = gridValue.GetValueNormalized();
                Vector2 gridUv = new Vector2(gridValueNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * 0.5f,
                    0, quadSize, gridUv, gridUv);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void Grid_OnGridObjectChanged(object sender, MyGrid<HeatMapGridObject>.OnGridObjectChangedEventArgs e)
    {
        meshUpdate = true;
    }

    private void LateUpdate()
    {
        if (meshUpdate)
        {
            meshUpdate = false;
            UpDateHeatMapVisual();
        }
    }
}
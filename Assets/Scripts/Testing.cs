using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    private Grid grid;

    [SerializeField] private Vector3 originPosition;
    void Start()
    {
        grid = new Grid(1, 1,10f,new Vector3(0,0));
        HeatMapVisual heatMapVisual = new HeatMapVisual(grid, GetComponent<MeshFilter>());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -1 * Camera.main.transform.position.z));
            grid.setValue(mousePosition, 56);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -1 * Camera.main.transform.position.z));
            Debug.Log(grid.GetValue(mousePosition));
        }
    }
    

    private class HeatMapVisual
    {
        private Grid grid;
        private Mesh mesh;

        public HeatMapVisual(Grid grid,MeshFilter meshFilter)
        {
            this.grid = grid;
            mesh = new Mesh();
            UpDateHeatMapVisual();
            Debug.Log(mesh.vertices[0]);
            Debug.Log(grid.GetWorldPosition(0,0));
            Debug.DrawLine(mesh.vertices[0], grid.GetWorldPosition(0, 0), Color.blue, 100f);
            meshFilter.mesh = mesh;
        }

        public void UpDateHeatMapVisual()
        {
            Vector3[] vertices;
            Vector2[] uv;
            int[] triangles;
            MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out vertices, out uv, out triangles);
            

            // vertices[0] = grid.GetWorldPosition(0, 0);
            // vertices[1] = grid.GetWorldPosition(0, 0) + new Vector3(0, grid.GetCellSize());
            // vertices[2] = grid.GetWorldPosition(0, 0) + new Vector3(grid.GetCellSize(), grid.GetCellSize());
            // vertices[3] = grid.GetWorldPosition(0, 0) + new Vector3(grid.GetCellSize(), 0);
            // uv[0] = new Vector2(0, 0);
            // uv[1] = new Vector2(0, 1);
            // uv[2] = new Vector2(1, 1);
            // uv[3] = new Vector2(1, 0);
            //
            // triangles[0] = 0;
            // triangles[1] = 1;
            // triangles[2] = 2;
            // triangles[3] = 0;
            // triangles[4] = 2;
            // triangles[5] = 3;
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    int index = x * grid.GetHeight() + y;
                    MeshUtils.AddToMeshArrays(vertices,uv,triangles,index,grid.GetWorldPosition(x,y),grid.GetCellSize());
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
    }
}

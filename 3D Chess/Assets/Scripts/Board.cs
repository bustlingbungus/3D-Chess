using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grid = new Cell[grid_dimensions.x,grid_dimensions.y,grid_dimensions.z];

        for (int x=0; x<grid_dimensions.x; x++) {
            float xpos = (2f*x)-7f;
            for (int y=0; y<grid_dimensions.y; y++) {
                float ypos = (2f*y)-7f;
                for (int z=0; z<grid_dimensions.z; z++) {
                    Cell cell = Instantiate(
                        cell_prefab, 
                        new Vector3(xpos,ypos,(2f*z)-7f), 
                        Quaternion.identity, 
                        transform.GetChild(0))
                            .GetComponent<Cell>();
                    cell.index = new Vector3Int(x, y, z);
                    // grid[x,y,z] = cell;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject cell_prefab;

    [ContextMenu("Toggle Indices")]
    public void ToggleIndices()
    {
        foreach (Transform cell in transform.GetChild(0)) {
            cell.GetComponent<Cell>().ToggleIndexDisplay();
        }
        // foreach (Cell cell in grid) {
        //     cell.ToggleIndexDisplay();
        // }
    }

    public Cell GetCellAt(Vector3Int index) {
        return GetCellAt(index.x, index.y, index.z);
    }
    public Cell GetCellAt(int x, int y, int z)
    {
        // z + gridz*y + gridy*gridz*x
        if (x<0||y<0||z<0 || x>=grid_dimensions.x||y>=grid_dimensions.y||z>=grid_dimensions.z) return null;
        int idx = z + grid_dimensions.z*(y + (grid_dimensions.y * x));
        return transform.GetChild(0).GetChild(idx).GetComponent<Cell>();
        // if (x>=8||y>=8||z>=8 || x<0||y<0||z<0) return null;
        // return grid[x,y,z];
    }

    // private Cell[,,] grid;

    [SerializeField]
    private Vector3Int grid_dimensions = new Vector3Int(8, 8, 8);
}

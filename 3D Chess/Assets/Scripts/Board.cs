using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = new Cell[grid_dimensions.x,grid_dimensions.y,grid_dimensions.z];

        var board = gameObject.transform.GetChild(0);
        int z = 0;
        foreach (Transform floor in board) {
            int y = 0;
            foreach (Transform rank in floor) {
                int x = 0;
                foreach (Transform file in rank) {
                    var cell = file.gameObject.GetComponent<Cell>();
                    cell.index = new Vector3Int(x, y, z);
                    grid[x,y,z] = cell;
                    x++;
                }
                y++;
            }
            z++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Toggle Indices")]
    public void ToggleIndices()
    {
        foreach (Cell cell in grid) {
            cell.ToggleIndexDisplay();
        }
    }

    public Cell GetCellAt(Vector3Int index) {
        return GetCellAt(index.x, index.y, index.z);
    }
    public Cell GetCellAt(int x, int y, int z)
    {
        if (x>=8||y>=8||z>=8 || x<0||y<0||z<0) return null;
        return gameObject.transform.GetChild(0)
                .GetChild(z).GetChild(y).GetChild(x)
                .GetComponent<Cell>();
        // return grid[x,y,z];
    }

    private Cell[,,] grid;

    [SerializeField]
    private Vector3Int grid_dimensions = new Vector3Int(8, 8, 8);
}

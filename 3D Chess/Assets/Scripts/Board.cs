using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

/// <summary>
/// Main storge for 3D array of cell objects.
/// </summary>
public class Board : MonoBehaviour
{
    /* ==========  MEMBERS  ========== */

    /// <summary> Prefab of the cell to be instantiated into the array. </summary>
    [SerializeField]
    private GameObject cell_prefab;
    /// <summary> xyz dimensions of the 3D array. </summary>
    public Vector3Int grid_dimensions = new Vector3Int(8, 8, 8);
    

    /// <summary> 3D array of cell objects. </summary>
    private Cell[,,] grid;




    /* ==========  MAIN FUNCTIONS  ========== */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // intialise array
        grid = new Cell[grid_dimensions.x,grid_dimensions.y,grid_dimensions.z];

        // iterate over every (x,y,z) coordinate triple. Instantiate a new cell for each coordinate. 
        for (int x=0; x<grid_dimensions.x; x++) 
        {
            // Cell positions will begin at -7 on each axis, range until 7, and have a step of 2.
            // this will make the board centred at (0,0,0)
            float xpos = (2f*x)-7f;
            for (int y=0; y<grid_dimensions.y; y++) 
            {
                float ypos = (2f*y)-7f;
                for (int z=0; z<grid_dimensions.z; z++) 
                {
                    // Instantiate cell prefab as a child of cell container, and access the Cell script
                    Cell cell = Instantiate(
                        cell_prefab, 
                        new Vector3(xpos,ypos,(2f*z)-7f), 
                        Quaternion.identity, 
                        transform.GetChild(0))
                            .GetComponent<Cell>();
                    // set the cell's indices
                    cell.index = new Vector3Int(x, y, z);

                    // commit hte finished cell to the array 
                    grid[cell.index.x,cell.index.y,cell.index.z] = cell;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get the cell at the given indices,
    /// </summary>
    /// <param name="index">xyz Vector3Int of the desired xyz indices</param>
    /// <returns>A reference to the cell at indices xyz. <c>null</c> if the indices are out of bounds.</returns>
    public Cell GetCellAt(Vector3Int index) {
        return GetCellAt(index.x, index.y, index.z);
    }

    /// <summary>
    /// Get the cell at the given indices.
    /// </summary>
    /// <param name="index">x, y, and z indices.</param>
    /// <returns>A reference to the cell at indices xyz. <c>null</c> if the indices are out of bounds.</returns>
    public Cell GetCellAt(int x, int y, int z)
    {
        // return null if index is out of bounds
        if (x<0||y<0||z<0 || x>=grid_dimensions.x||y>=grid_dimensions.y||z>=grid_dimensions.z) return null;
        return grid[x,y,z];
    }




    /* ==========  HELPER FUNCTIONS  ========== */

    /// <summary>
    /// Toggles the index rendering for all cells in the grid.
    /// </summary>
    [ContextMenu("Toggle Indices")]
    public void ToggleIndices()
    {
        foreach (Cell cell in grid) cell.ToggleIndexDisplay();
    }
}

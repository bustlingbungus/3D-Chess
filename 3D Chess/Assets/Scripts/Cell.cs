using System.Collections.Generic;
using System.Threading.Tasks;
using Defs;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// A container for a piece. Contains an index, a <c>Vector3Int</c> representing its xyz indices in a 3D array. 
/// A 3D array of cells is stored in the global <c>Board</c> object, and cells with adjacent indices are adjacent 
/// in said array, as well as in world space. 
/// </summary>
public class Cell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialise attackers 
        attackers = new Dictionary<TeamColour,List<Piece>>();
        // i know it says initialisation can be simplified, but trust me the alternative is ugly and less simple.
        attackers.Add(TeamColour.White, new List<Piece>());
        attackers.Add(TeamColour.Black, new List<Piece>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Shows or hides the cell's index viewer, by enabling/disabling the child object.
    /// </summary>
    [ContextMenu("Toggle Index")]
    public void ToggleIndexDisplay() {
        var obj = gameObject.transform.GetChild(0).gameObject;
        obj.SetActive(!obj.activeSelf);
        if (obj.activeSelf) obj.GetComponent<CellIndex>().UpdateText(index);
    }

    /// <summary> The cell's xyz indices in the global 3D cell array. </summary>
    public Vector3Int index = Vector3Int.zero;

    /// <summary> Reference to the piece within the cell. <c>null</c> if the cell is empty. </summary>
    public Piece occupant = null;

    /// <summary> Hash table, keyed by colour, values are list of pieces that are able to capture this cell. </summary>
    public Dictionary<TeamColour,List<Piece>> attackers;
}

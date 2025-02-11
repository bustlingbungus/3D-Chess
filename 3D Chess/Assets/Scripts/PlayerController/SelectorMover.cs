using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// <para>Moves around the cell selctor from cell to cell, based on user input and camera facing direction.</para>
/// </summary>
public class SelectorMover : MonoBehaviour
{
    /// <summary> Reference to the parent selctor to move around. </summary>
    public CellSelector selector;

    /// <summary> Keybindings for movement directions. </summary>
    [SerializeField]
    private KeyCode positiveX = KeyCode.W,
                    negativeX = KeyCode.S,
                    positiveY = KeyCode.UpArrow,
                    negativeY = KeyCode.DownArrow,
                    positiveZ = KeyCode.A,
                    negativeZ = KeyCode.D;
    
    /// <summary> Reference to the main board object. </summary>
    private Board board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get reference to board
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        // default selector cell to 0, 0, 0
        if (selector.Cell==null) selector.Cell = board.GetCellAt(0, 0, 0);
        else {
            // get basis vectors such that one basis is the camera's facing direction
            Vector3 base_z = Camera.main.transform.forward, base_y=Vector3.zero, base_x=Vector3.zero;
            Vector3.OrthoNormalize(ref base_z, ref base_y, ref base_x);

            // correct the x and y basis vectors to be more intuitive at weird angles
            if (base_z.z!=0f) { 
                base_x*=-1f; 
                base_y*=-1f; 
            } else if (base_z.y != 0f) {
                Vector3 temp = base_x;
                base_x = base_y;
                base_y = temp;
                if (base_z.y==-1) base_x *= -1;
                else base_y *= -1;
            }

            Vector3 index = selector.Cell.index;

            // check for input, and edit the magnitude/direction of the basis vectors before adding them to the index
            if (Input.GetKeyDown(negativeZ)) base_z *= -1f;
            else if (!Input.GetKeyDown(positiveZ)) base_z = Vector3.zero;
            if (Input.GetKeyDown(negativeX)) base_x *= -1f;
            else if (!Input.GetKeyDown(positiveX)) base_x = Vector3.zero;
            if (Input.GetKeyDown(negativeY)) base_y *= -1f;
            else if (!Input.GetKeyDown(positiveY)) base_y = Vector3.zero;

            // add modified basis vectors to the cell index, and convert to integer indices
            index += base_z + base_y + base_x;
            Vector3Int idx = new Vector3Int((int)index.x,(int)index.y,(int)index.z);

            // if a new index was selected, update selector position
            if (idx!=selector.Cell.index) selector.Cell = board.GetCellAt(idx);
        }
    }
}

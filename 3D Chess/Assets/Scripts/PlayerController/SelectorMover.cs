using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// <para>Moves around the cell selctor from cell to cell, based on user input and camera facing direction.</para>
/// </summary>
public class SelectorMover : MonoBehaviour
{
    /// <summary> Reference to the parent selctor to move around. </summary>
    public CellSelector selector;
    
    /// <summary> Reference to the main board object. </summary>
    private Board board;
    private InputManager input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get reference to board
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
        input = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // default selector cell to 0, 0, 0
        if (selector.Cell==null) selector.Cell = board.GetCellAt(0, 0, 0);
        else {

            // get basis vectors based on the camera's facing direction
            Vector3 fb = Camera.main.transform.forward,
                    lr = Camera.main.transform.right * -1f,
                    ud = Camera.main.transform.up;

            // check for input, and edit the magnitude/direction of the basis vectors before adding them to the index
            if (input.MoveBackwards) fb *= -1f;
            else if (!input.MoveForwards) fb = Vector3.zero;
            if (input.MoveRight) lr *= -1f;
            else if (!input.MoveLeft) lr = Vector3.zero;
            if (input.MoveDown) ud *= -1f;
            else if (!input.MoveUp) ud = Vector3.zero;

            // make each of the axes have one nonzero member
            correctValues(ref fb);
            correctValues(ref lr);
            correctValues(ref ud);

            // add modified basis vectors to the cell index, and convert to integer indices
            Vector3 index = selector.Cell.index + fb + lr + ud;
            Vector3Int idx = new Vector3Int((int)index.x,(int)index.y,(int)index.z);

            // if a new index was selected, update selector position
            if (idx!=selector.Cell.index) selector.Cell = board.GetCellAt(idx);
        }
    }

    /// <summary> 
    /// Modifies a vector to only have one nonzero element: the element with the greatest magnitude in the unmodified vector.
    /// Makes the nonzero element have a magnitude of 1, with the same sign as said element in the unmodified vector.
    /// </summary>
    /// <param name="vec">The vector to modify.</param>
    private void correctValues(ref Vector3 vec)
    {
        // x is greatest
        if (Mathf.Abs(vec.x)>Mathf.Abs(vec.y) && Mathf.Abs(vec.x)>Mathf.Abs(vec.z)) {
            vec = new Vector3(Mathf.Sign(vec.x), 0f, 0f);
        // y is greatest
        } else if (Mathf.Abs(vec.y)>Mathf.Abs(vec.x) && Mathf.Abs(vec.y)>Mathf.Abs(vec.z)) {
            vec = new Vector3(0f, Mathf.Sign(vec.y), 0f);
        // z is greatest, or vector is 0
        } else vec = vec==Vector3.zero? Vector3.zero : new Vector3(0f, 0f, Mathf.Sign(vec.z));
    }
}

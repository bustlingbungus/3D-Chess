using UnityEngine;

public class SelectorMover : MonoBehaviour
{
    public CellSelector selector;
    private Board board;

    [SerializeField]
    private KeyCode positiveX = KeyCode.W,
                   negativeX = KeyCode.S,
                   positiveY = KeyCode.UpArrow,
                   negativeY = KeyCode.DownArrow,
                   positiveZ = KeyCode.A,
                   negativeZ = KeyCode.D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selector.Cell==null) selector.Cell = board.GetCellAt(0, 0, 0);
        else {
            Vector3Int index = selector.Cell.index;
            if (Input.GetKeyDown(positiveX)) selector.Cell = board.GetCellAt(++index.x, index.y, index.z);
            if (Input.GetKeyDown(negativeX)) selector.Cell = board.GetCellAt(--index.x, index.y, index.z);
            if (Input.GetKeyDown(positiveY)) selector.Cell = board.GetCellAt(index.x, ++index.y, index.z);
            if (Input.GetKeyDown(negativeY)) selector.Cell = board.GetCellAt(index.x, --index.y, index.z);
            if (Input.GetKeyDown(positiveZ)) selector.Cell = board.GetCellAt(index.x, index.y, ++index.z);
            if (Input.GetKeyDown(negativeZ)) selector.Cell = board.GetCellAt(index.x, index.y, --index.z);
        }   
    }
}

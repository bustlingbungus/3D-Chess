using UnityEngine;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode positiveX = KeyCode.W,
                   negativeX = KeyCode.X,
                   positiveY = KeyCode.UpArrow,
                   negativeY = KeyCode.DownArrow,
                   positiveZ = KeyCode.D,
                   negativeZ = KeyCode.S;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cell==null) Cell = board.GetCellAt(0, 0, 0);
        else {
            Vector3Int index = cell.index;
            if (Input.GetKeyDown(positiveX)) Cell = board.GetCellAt(++index.x, index.y, index.z);
            if (Input.GetKeyDown(negativeX)) Cell = board.GetCellAt(--index.x, index.y, index.z);
            if (Input.GetKeyDown(positiveY)) Cell = board.GetCellAt(index.x, ++index.y, index.z);
            if (Input.GetKeyDown(negativeY)) Cell = board.GetCellAt(index.x, --index.y, index.z);
            if (Input.GetKeyDown(positiveZ)) Cell = board.GetCellAt(index.x, index.y, ++index.z);
            if (Input.GetKeyDown(negativeZ)) Cell = board.GetCellAt(index.x, index.y, --index.z);
        }
    }

    public Cell Cell
    {
        get => cell;
        set {
            if (value!=null) {
                if (cell!=null && cell.occupant!=null) cell.occupant.HideMoves();
                cell = value;
                transform.position = cell.transform.position;
                if (cell.occupant!=null) cell.occupant.ShowMoves();
            }
        }
    }

    private Cell cell;
    private Board board;
}

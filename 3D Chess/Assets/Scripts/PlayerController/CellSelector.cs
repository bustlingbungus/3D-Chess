using UnityEngine;
using Defs;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode select = KeyCode.Return;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cell_select_input())
        {
            LowerAlpha();
            move_select.enabled = true;
            movement_controls.enabled = false;
            move_select.GetMoves(cell.occupant);
        }
    }

    public Cell Cell
    {
        get => cell;
        set
        {
            if (value != null)
            {
                if (cell != null && cell.occupant != null) cell.occupant.HideMoves();
                cell = value;
                transform.position = cell.transform.position;
                if (cell.occupant != null && cell.occupant.Colour == current_player) cell.occupant.ShowMoves();
            }
        }
    }

    private TeamColour current_player = TeamColour.White;

    private Cell cell;

    [SerializeField]
    private PieceMover move_select;
    [SerializeField]
    private SelectorMover movement_controls;

    private Board _board;



    [SerializeField]
    private Material material;


    [ContextMenu("Lower Alpha")]
    public void LowerAlpha() { SetHighlightAlpha(0.15f); }
    [ContextMenu("Raise Alpha")]
    public void RaiseAlpha() { SetHighlightAlpha(0.588f); }

    public void SetHighlightAlpha(float newAlpha)
    {
        Color newCol = material.color;
        newCol.a = newAlpha;
        material.color = newCol;
        if (cell.occupant != null)
        {
            foreach (Transform child in cell.occupant.transform)
            {
                if (child.tag == "Cell Highlight")
                {
                    Material mat = child.GetComponent<MeshRenderer>().material;
                    newCol = mat.color;
                    newCol.a = newAlpha;
                    mat.color = newCol;
                }
            }
        }
    }


    [ContextMenu("Change Turn")]
    public void ChangeTurn()
    {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;
        current_player = current_player == TeamColour.White ? TeamColour.Black : TeamColour.White;
        _board.RegenerateMoves();
        LookForStalemate();
    }

    public void ExitSelection()
    {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;
    }

    private bool cell_select_input()
    {
        return Input.GetKeyDown(select) && !move_select.enabled &&
               cell != null && cell.occupant != null && cell.occupant.Colour == current_player &&
               !IsDiscoveredCheck(cell);
    }

    [ContextMenu("Look for checkmate")]
    void LookForStalemate()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        bool inCheck = false;
        TeamColour opposite = current_player == TeamColour.White ? TeamColour.Black : TeamColour.White;
        foreach (GameObject obj in pieces)
        {
            Piece piece = obj.GetComponent<Piece>();
            if (piece.Colour == current_player)
            {
                if (piece.available_moves.Count > 0)
                {
                    Debug.Log("Not Checkmate");
                    return;
                }
                else if (piece.Type == PieceType.King)
                {
                    inCheck = piece.Cell.attackers[opposite].Count > 0;
                }
            }
        }
        if (inCheck) Debug.Log("Checkmate!");
        else Debug.Log("Stalemate!");
    }
    private bool checkForInCheck()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        TeamColour opposite = current_player == TeamColour.White ? TeamColour.Black : TeamColour.White;
        foreach (GameObject obj in pieces)
        {
            Piece piece = obj.GetComponent<Piece>();
            if (piece.Colour == opposite)
            {
                foreach (MoveInfo move in piece.available_moves)
                {
                    if (move.cell.occupant != null && move.cell.occupant.Type == PieceType.King)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsDiscoveredCheck(Cell targetCell)
    {
        // Temporarily move the piece to the target cell
        Piece originalPiece = cell.occupant;
        Piece targetPiece = targetCell.occupant;
        targetCell.occupant = originalPiece;
        cell.occupant = null;

        bool inCheck = checkForInCheck();

        // Revert the move
        cell.occupant = originalPiece;
        targetCell.occupant = targetPiece;

        return inCheck;
    }
}

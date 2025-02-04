using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King,
};

public enum TeamColour
{
    White,
    Black
};

public abstract class Piece : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init();
    }

    // Update is called once per frame
    void Update()
    {
        piece_update();
    }

    public abstract List<Cell> find_valid_moves();

    public GameObject validMoveHighlightPrefab;

    public void piece_init()
    {
        // find board
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
        // find closest cell
        look_for_cell();
    }

    public void piece_update()
    {
        if (cell==null) look_for_cell();
    }

    private void look_for_cell()
    {
        // find closest cell
        Cell best_cell = null;
        float best_dist = float.MaxValue;

        foreach (Transform cell in board.transform.GetChild(0)) {
            float disp = (cell.transform.position-transform.position).magnitude;
            if (disp < best_dist) {
                best_dist = disp;
                best_cell = cell.GetComponent<Cell>();
            }
        }
        if (best_cell != null) {
            cell = best_cell;
            transform.position = best_cell.transform.position;
        }
    }

    [HideInInspector]
    public Board board;

    public PieceType Type 
    {
        get => type;
        set => type = value;
    }

    public TeamColour Colour 
    {
        get => colour;
        set => colour = value;
    }

    public Cell Cell
    {
        get => cell;
        set {
            cell = value;
            transform.position = cell.transform.position;
        }
    }


    private Cell cell;
    private PieceType type = PieceType.Pawn;
    [SerializeField]
    private TeamColour colour = TeamColour.White;


    [ContextMenu("Show Moves")]
    public void ShowMoves() {
        List<Cell> moves = this.find_valid_moves();
        foreach (Cell cell in moves) Instantiate(validMoveHighlightPrefab, cell.transform);
    }
}

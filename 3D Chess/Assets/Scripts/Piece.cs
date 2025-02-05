using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;

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

public struct Move
{
    public Move(Cell cell_ref, MoveType type) { cell = cell_ref; move_type = type; }
    public Cell cell;
    public enum MoveType { Regular, Attack };
    public MoveType move_type;
};

public abstract class Piece : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public abstract List<Move> find_valid_moves();

    public GameObject validMoveHighlightPrefab;
    public GameObject attackMoveHighlightPrefab;

    public void piece_init()
    {
        if (colour == TeamColour.Black) transform.eulerAngles = new Vector3(0f, 180f, 0f);
        else transform.eulerAngles = Vector3.zero;

        movement_timer = travelTime;

        // find board
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
        // find closest cell
        look_for_cell();
    }

    public void piece_update()
    {
        if (Cell==null) look_for_cell();

        transform.position = pos_interp(init_pos, cell.transform.position, movement_timer/travelTime);
        movement_timer = Mathf.Clamp(movement_timer+Time.deltaTime, 0f, travelTime);
    }

    public void look_for_cell()
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
        if (best_cell != null) Cell = best_cell;
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
        set {
            colour = value;
            if (colour == TeamColour.Black) transform.eulerAngles = new Vector3(0f, 180f, 0f);
            else transform.eulerAngles = Vector3.zero;
        }
    }

    public Cell Cell
    {
        get => cell;
        set {
            if (value != null) {
                if (cell!=null) {
                    cell.occupant = null;
                    init_pos = cell.transform.position;
                }
                cell = value;
                if (cell.occupant!=null) Destroy(cell.occupant.gameObject);
                cell.occupant = this;
                movement_timer = 0f;
            }
        }
    }


    [SerializeField]
    private float travelTime = 0.75f;
    private float movement_timer = 0f;
    private Vector3 init_pos = Vector3.zero;
    private Vector3 pos_interp(Vector3 a, Vector3 b, float t) {
        t = 0.5f*(Mathf.Sin(Mathf.PI*(t-0.5f))+1f);
        return a + t*(b - a);
    }



    private Cell cell;
    private PieceType type = PieceType.Pawn;
    [SerializeField]
    private TeamColour colour = TeamColour.White;


    [ContextMenu("Show Moves")]
    public void ShowMoves() {
        List<Move> moves = find_valid_moves();
        foreach (Move move in moves) {
            if (move.move_type==Move.MoveType.Regular) Instantiate(validMoveHighlightPrefab, move.cell.transform.position, Quaternion.identity, transform);
            else if (move.move_type == Move.MoveType.Attack) Instantiate(attackMoveHighlightPrefab, move.cell.transform.position, Quaternion.identity, transform);
        }
    }

    [ContextMenu("Hide Moves")]
    public void HideMoves() {
        foreach (Transform child in transform) {
            if (child.tag == "Cell Highlight") Destroy(child.gameObject);
        }
    }
}

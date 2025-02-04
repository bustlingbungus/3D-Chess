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
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract List<Cell> find_valid_moves();

    public GameObject valid_move_highlight_prefab;


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


    [SerializeField]
    private Cell cell;
    private PieceType type = PieceType.Pawn;
    [SerializeField]
    private TeamColour colour = TeamColour.White;


    [ContextMenu("Show Moves")]
    public void ShowMoves() {
        List<Cell> moves = this.find_valid_moves();
        foreach (Cell cell in moves) Instantiate(valid_move_highlight_prefab, cell.transform);
    }
}

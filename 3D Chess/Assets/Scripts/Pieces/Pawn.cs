using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Type = PieceType.Pawn;
        piece_init();
    }

    // Update is called once per frame
    void Update()
    {
        piece_update();
    }

    public override List<Cell> find_valid_moves()
    {
        List<Cell> res = new List<Cell>();
        Vector3Int index = Cell.index;

        // forwards movement
        index.x += (Colour==TeamColour.White)? 1 : -1;
        Cell cell = board.GetCellAt(index);
        if (cell!=null) res.Add(cell);

        index = Cell.index;
        // vertical movement
        index.y += (Colour==TeamColour.White)? -1 : 1;
        cell = board.GetCellAt(index);
        if (cell!=null) res.Add(cell);

        return res;
    }
}

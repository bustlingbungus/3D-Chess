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

    public override List<Move> find_valid_moves()
    {
        List<Move> res = new List<Move>();
        Vector3Int index = Cell.index;

        // forwards movement
        index.x += (Colour==TeamColour.White)? 1 : -1;
        Cell cell = board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) res.Add(new Move(cell, Move.MoveType.Regular));

        index = Cell.index;
        // vertical movement
        index.y += (Colour==TeamColour.White)? -1 : 1;
        cell = board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) res.Add(new Move(cell, Move.MoveType.Regular));

        // diagonal attack 1
        index.x += (Colour==TeamColour.White)? 1 : -1;
        index.z += 1;
        cell = board.GetCellAt(index);        
        if (cell!=null && cell.occupant!=null && cell.occupant.Colour!=Colour) res.Add(new Move(cell, Move.MoveType.Attack));
        
        index.z -= 2;
        cell = board.GetCellAt(index);        
        if (cell!=null && cell.occupant!=null && cell.occupant.Colour!=Colour) res.Add(new Move(cell, Move.MoveType.Attack));

        return res;
    }
}

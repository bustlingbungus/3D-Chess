using System.Collections.Generic;
using UnityEngine;
using Defs;

public class Pawn : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.Pawn);
    }

    public override List<Cell> find_valid_moves()
    {
        List<Cell> res = new List<Cell>();
        Vector3Int index = Cell.index;

        // forwards movement
        index.x += (Colour==TeamColour.White)? 1 : -1;
        Cell cell = _board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) res.Add(cell);

        index = Cell.index;
        // vertical movement
        index.y += (Colour==TeamColour.White)? -1 : 1;
        cell = _board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) res.Add(cell);

        // diagonal attack 1
        index.x += (Colour==TeamColour.White)? 1 : -1;
        index.z += 1;
        cell = _board.GetCellAt(index);
        if (cell != null) {
            cell.attackers[Colour].Add(this);
            if (cell.occupant!=null && cell.occupant.Colour != Colour) res.Add(cell);
        }
        
        index.z -= 2;
        cell = _board.GetCellAt(index);
        if (cell != null) {
            cell.attackers[Colour].Add(this);
            if (cell.occupant!=null && cell.occupant.Colour != Colour) res.Add(cell);
        }

        return res;
    }
}

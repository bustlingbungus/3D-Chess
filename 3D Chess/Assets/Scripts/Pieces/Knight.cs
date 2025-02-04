using UnityEngine;
using System.Collections.Generic;

public class Knight : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Type = PieceType.Knight;
        piece_init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cell==null) look_for_cell();
    }

    public override List<Move> find_valid_moves()
    {
        List<Move> res = new List<Move>();

        // x +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i<4?-2:2, i%2==0?i%4==0?-1:1:0, i%2==1?i%4==1?-1:1:0);
            Cell cell = board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                if (cell.occupant==null) res.Add(new Move(cell, Move.MoveType.Regular));
                else if (cell.occupant.Colour!=Colour) res.Add(new Move(cell, Move.MoveType.Attack));
            }
        }
        // y +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i%2==0?i%4==0?-1:1:0, i<4?-2:2, i%2==1?i%4==1?-1:1:0);
            Cell cell = board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                if (cell.occupant==null) res.Add(new Move(cell, Move.MoveType.Regular));
                else if (cell.occupant.Colour!=Colour) res.Add(new Move(cell, Move.MoveType.Attack));
            }
        }
        // z +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i%2==0?i%4==0?-1:1:0, i%2==1?i%4==1?-1:1:0, i<4?-2:2);
            Cell cell = board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                if (cell.occupant==null) res.Add(new Move(cell, Move.MoveType.Regular));
                else if (cell.occupant.Colour!=Colour) res.Add(new Move(cell, Move.MoveType.Attack));
            }
        }

        return res;
    }
}

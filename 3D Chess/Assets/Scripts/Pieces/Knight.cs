using UnityEngine;
using System.Collections.Generic;
using Defs;

public class Knight : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.Knight);
    }

    public override List<Cell> find_valid_moves()
    {
        List<Cell> res = new List<Cell>();

        // x +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i<4?-2:2, i%2==0?i%4==0?-1:1:0, i%2==1?i%4==1?-1:1:0);
            Cell cell = _board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                cell.attackers[Colour].Add(this);
                if (cell.occupant==null||cell.occupant.Colour!=Colour) res.Add(cell);
            }
        }
        // y +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i%2==0?i%4==0?-1:1:0, i<4?-2:2, i%2==1?i%4==1?-1:1:0);
            Cell cell = _board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                cell.attackers[Colour].Add(this);
                if (cell.occupant==null||cell.occupant.Colour!=Colour) res.Add(cell);
            }
        }
        // z +/-2 routes
        for (int i=0; i<8; i++) {
            Vector3Int disp = new Vector3Int(i%2==0?i%4==0?-1:1:0, i%2==1?i%4==1?-1:1:0, i<4?-2:2);
            Cell cell = _board.GetCellAt(Cell.index + disp);
            if (cell != null) {
                cell.attackers[Colour].Add(this);
                if (cell.occupant==null||cell.occupant.Colour!=Colour) res.Add(cell);
            }
        }

        return res;
    }
}

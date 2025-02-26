using UnityEngine;
using System.Collections.Generic;
using Defs;

public class King : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.King);
    }

    public override List<Cell> find_valid_moves()
    {
        List<Cell> res = new List<Cell>();
        TeamColour opposite_colour = Colour == TeamColour.White ? TeamColour.Black : TeamColour.White;

        // orthogonal movements
        for (int i = 0; i < 6; i++)
        {
            // find displacement by i's value
            Vector3Int disp = new Vector3Int(i < 2 ? 1 : 0, i < 4 && i > 1 ? 1 : 0, i < 6 && i > 3 ? 1 : 0);
            if (i % 2 == 1) disp *= -1;

            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index + disp);
            if (curr != null)
            {
                curr.attackers[Colour].Add(this);
                if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
            }
        }

        // diagonals
        for (int i = 0; i < 8; i++)
        {
            // find displacement with binary counting
            Vector3Int disp = new Vector3Int((i & 4) != 0 ? 1 : -1, (i & 2) != 0 ? 1 : -1, (i & 1) != 0 ? 1 : -1);

            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index + disp);
            if (curr != null)
            {
                curr.attackers[Colour].Add(this);
                if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
            }
        }

        // 2 dimensional diagonals
        for (int i = 0; i < 12; i++)
        {
            Vector3Int disp = new Vector3Int(i < 8 ? i < 4 ? -1 : 1 : 0, i < 8 ? (i % 2) == 1 ? (i % 4) == 1 ? -1 : 1 : 0 : i < 10 ? -1 : 1, i < 8 ? (i % 2) == 0 ? (i % 4) == 0 ? -1 : 1 : 0 : (i % 2) == 0 ? -1 : 1);
            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index + disp);
            if (curr != null)
            {
                curr.attackers[Colour].Add(this);
                if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
            }
        }
        
        // if (move_cnt == 0)
        // {
        //     // castling
        //     for (int i = 0; i < 2; i++)
        //     {
        //         Vector3Int disp = new Vector3Int(i == 0 ? 2 : -2, 0, 0);
        //         Cell curr = _board.GetCellAt(Cell.index + disp);
        //         if (curr != null && curr.occupant == null)
        //         {
        //             bool can_castle = true;
        //             for (int j = 0; j < 3; j++)
        //             {
        //                 Cell check = _board.GetCellAt(Cell.index + new Vector3Int(i == 0 ? 1 : -1, 0, 0) * j);
        //                 if (check != null && check.attackers[opposite_colour].Count > 0) can_castle = false;
        //             }
        //             if (can_castle) res.Add(curr);
        //         }
        //     }
        // }

        return res;
    }
}

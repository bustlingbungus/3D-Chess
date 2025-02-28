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

        // 9 moves in +x
        Vector3Int index = Cell.index + new Vector3Int(1,-1,-1);
        for (int i=1; i<=9; i++)
        {
            // check current cell
            Cell curr = _board.GetCellAt(index);
            if (curr != null)
            {
                curr.attackers[Colour].Add(this);
                if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
            }

            if (i%3 == 0) {
                index.y++; index.z -= 2;
            } else index.z++;
        }

        // 8 moves in the +0x plane
        index = Cell.index + new Vector3Int(0,-1,-1);
        for (int i=1; i<=9; i++)
        {
            if (index != Cell.index) 
            {
                // check current cell
                Cell curr = _board.GetCellAt(index);
                if (curr != null)
                {
                    curr.attackers[Colour].Add(this);
                    if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
                }
            }


            if (i%3 == 0) {
                index.y++; index.z -= 2;
            } else index.z++;
        }

        // 9 moves in -x
        index = Cell.index + new Vector3Int(-1,-1,-1);
        for (int i=1; i<=9; i++)
        {
            // check current cell
            Cell curr = _board.GetCellAt(index);
            if (curr != null)
            {
                curr.attackers[Colour].Add(this);
                if (curr.occupant == null || curr.occupant.Colour != Colour) res.Add(curr);
            }

            if (i%3 == 0) {
                index.y++; index.z -= 2;
            } else index.z++;
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

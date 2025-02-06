using UnityEngine;
using System.Collections.Generic;
using Defs;

public class Bishop : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.Bishop);
    }

    // Update is called once per frame
    void Update()
    {
        piece_update();
    }

    public override List<Move> find_valid_moves()
    {
        List<Move> res = new List<Move>();

        bool index_in_bounds(Vector3Int idx) {
            return idx.x>=0 && idx.y>=0 && idx.z>=0 &&
                   idx.x<_board.grid_dimensions.x &&
                   idx.y<_board.grid_dimensions.y &&
                   idx.z<_board.grid_dimensions.z;
        }

        // diagonals
        for (int i=0; i<8; i++)
        {
            // find displacement with binary counting
            Vector3Int disp = new Vector3Int((i&4)!=0?1:-1,(i&2)!=0?1:-1,(i&1)!=0?1:-1);

            // check current displacement
            for (Vector3Int index = Cell.index+disp;index_in_bounds(index);index += disp)
            {
                Cell curr = _board.GetCellAt(index);
                if (curr==null) continue;
                if (curr.occupant!=null) {
                    if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                    break;
                }
                res.Add(new Move(curr, Move.MoveType.Regular));
            }
        }
        return res;
    }
}

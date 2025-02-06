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

    // Update is called once per frame
    void Update()
    {
        piece_update();
    }

    public override List<Move> find_valid_moves()
    {
        List<Move> res = new List<Move>();

        // orthogonal movements
        for (int i=0; i<6; i++)
        {
            // find displacement by i's value
            Vector3Int disp = new Vector3Int(i<2?1:0,i<4&&i>1?1:0,i<6&&i>3?1:0);
            if (i%2==1) disp *= -1;

            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index+disp);
            if (curr==null) continue;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }

        // diagonals
        for (int i=0; i<8; i++)
        {
            // find displacement with binary counting
            Vector3Int disp = new Vector3Int((i&4)!=0?1:-1,(i&2)!=0?1:-1,(i&1)!=0?1:-1);

            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index+disp);
            if (curr==null) continue;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                continue;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }

        // 2 dimensional diagonals
        for (int i=0; i<12; i++)
        {
            Vector3Int disp = new Vector3Int(i<8?i<4?-1:1:0,i<8?(i%2)==1?(i%4)==1?-1:1:0:i<10?-1:1,i<8?(i%2)==0?(i%4)==0?-1:1:0:(i%2)==0?-1:1);
            // check current displacement
            Cell curr = _board.GetCellAt(Cell.index+disp);
            if (curr==null) continue;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                continue;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }

        return res;
    }
}

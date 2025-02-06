using UnityEngine;
using System.Collections.Generic;
using Defs;

public class Queen : Piece
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.Queen);
    }

    // Update is called once per frame
    void Update()
    {
        piece_update();
    }

    public override List<Move> find_valid_moves()
    {
        List<Move> res = new List<Move>();

        // negative along x axis
        for (Vector3Int index=Cell.index+new Vector3Int(-1,0,0); index.x>=0; index.x--)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }
        // positive along x axis
        for (Vector3Int index=Cell.index+new Vector3Int(1,0,0); index.x<_board.grid_dimensions.x; index.x++)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }
        
        // negative along y axis
        for (Vector3Int index=Cell.index+new Vector3Int(0,-1,0); index.y>=0; index.y--)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }
        // positive along y axis
        for (Vector3Int index=Cell.index+new Vector3Int(0,1,0); index.y<_board.grid_dimensions.y; index.y++)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }
        
        // negative along z axis
        for (Vector3Int index=Cell.index+new Vector3Int(0,0,-1); index.z>=0; index.z--)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
            if (curr.occupant!=null) {
                if (curr.occupant.Colour!=Colour) res.Add(new Move(curr, Move.MoveType.Attack));
                break;
            }
            res.Add(new Move(curr, Move.MoveType.Regular));
        }
        // positive along z axis
        for (Vector3Int index=Cell.index+new Vector3Int(0,0,1); index.z<_board.grid_dimensions.z; index.z++)
        {
            Cell curr = _board.GetCellAt(index);
            if (curr==null) break;
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
            Vector3Int disp = new Vector3Int(
                (i&4)!=0? 1 : -1,
                (i&2)!=0? 1 : -1,
                (i&1)!=0? 1 : -1
            );

            // check current displacement
            for (Vector3Int index = Cell.index+disp;index.x<_board.grid_dimensions.x&&index.y<_board.grid_dimensions.y&&index.z<_board.grid_dimensions.z;index += disp)
            {
                Cell curr = _board.GetCellAt(index);
                if (curr==null) break;
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

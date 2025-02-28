using System.Collections.Generic;
using UnityEngine;
using Defs;

// WHITE: x = 7 or y = 0
// BLACK: x = 0 or y = 7

public class Pawn : Piece
{
    [SerializeField]
    GameObject queenPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        piece_init(PieceType.Pawn);
    }

    void Update()
    {
        piece_update();

        if (Cell!=null) 
        {
            int x, y;
            if (Colour == TeamColour.White) {
                x = 7; y = 0;
            } else {
                x = 0; y = 7;
            }
            
            if (Cell.index.x==x || Cell.index.y==y)
            {
                Instantiate(queenPrefab, Cell.transform.position, Quaternion.identity, transform.parent);
                Destroy(gameObject);
                _board.RegenerateMoves(true);
            }
        }
    }

    public override List<Cell> find_valid_moves()
    {
        List<Cell> res = new List<Cell>();
        Vector3Int index = Cell.index;

        // forwards movement
        int dir = Colour==TeamColour.White? 1 : -1;
        index.x += dir;
        Cell cell = _board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) {
            res.Add(cell);
            // double first move
            if (move_cnt==0 && cell!=null) {
                index.x += dir;
                cell = _board.GetCellAt(index);
                if (cell!=null && cell.occupant==null) res.Add(cell);
                index.x -= dir;
            }
        }

        index = Cell.index;
        // vertical movement
        index.y -= dir;
        cell = _board.GetCellAt(index);
        if (cell!=null && cell.occupant==null) 
        {
            res.Add(cell);
            // double first move
            if (move_cnt==0 && cell!=null) {
                index.y -= dir;
                cell = _board.GetCellAt(index);
                if (cell!=null && cell.occupant==null) res.Add(cell);
                index.y += dir;
            }
        }

        // diagonal attacks
        index.x += dir;
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

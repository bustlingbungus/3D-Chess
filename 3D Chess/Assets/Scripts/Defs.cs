using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using System;

namespace Defs
{
    /// <summary> Different types of pieces. </summary>
    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
    };


    /// <summary> The different player colours. </summary>

    public enum TeamColour
    {
        White,
        Black
    };

    /// <summary>
    /// Container for information about where a piece may move, including the move destination, and 
    /// whether the move is a capture, or regular movement.
    /// </summary>
    public struct Move
    {
        public Move(Cell cell_ref, MoveType type) { cell = cell_ref; move_type = type; }
        /// <summary> Reference to the destination cell. </summary>
        public Cell cell;
        /// <summary> For determining if a move is a capture or regular movement. </summary>
        public enum MoveType { Regular, Attack };
        /// <summary> Whether or not the move is a capture or regular movement. </summary>
        public MoveType move_type;
    };

    public struct MoveInfo
    {
        public MoveInfo (Cell dest, Piece src, GameObject Indicator = null) {
            cell = dest; piece = src; indicator = Indicator;
        }
        public Cell cell;
        public GameObject indicator;
        public Piece piece;
        public bool isAttack() { return cell.occupant != null; }
    }



    /// <summary>
    /// Node in the camera node graph. Contains a position,  and index of itself in the adjacency graph.
    /// </summary>
    public class CameraNode
    {
        public CameraNode(Vector3 pos, int Idx) { position = pos; idx = Idx; }
        /// <summary> Camera position. </summary>
        public Vector3 position;
        /// <summary> Index of the node in graph. </summary>
        public int idx;
    }

    [System.Serializable]
    public class SpawnInfo
    {
        public SpawnInfo(Vector3Int Index, PieceType Type, TeamColour Colour) {
            type = Type; colour = Colour;
            pos = (2*Index) - new Vector3(7f,7f,7f);
        }
        public SpawnInfo(Vector3 Position, PieceType Type, TeamColour Colour) {
            type = Type; colour = Colour; pos = Position;
        }
        public Vector3 pos;
        public PieceType type;
        public TeamColour colour;
    }
}

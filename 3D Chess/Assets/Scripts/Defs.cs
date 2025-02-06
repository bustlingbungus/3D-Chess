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



    /// <summary>
    /// Node in the camera node graph. Contains a position and rotation
    /// </summary>
    public class CameraNode
    {
        public CameraNode(Vector3 pos, Vector3 rot, int Idx) { position = pos; eulerAngles = rot; idx = Idx; }
        public Vector3 position;
        public Vector3 eulerAngles;
        public int idx;
    }
}

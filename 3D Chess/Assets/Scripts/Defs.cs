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
    /// Information about a potential move.
    /// </summary>
    public struct MoveInfo
    {
        /// <summary>
        /// Information about a potential move.
        /// </summary>
        /// <param name="dest">Reference to the move's target cell</param>
        /// <param name="src">Reference to the piece being moved</param>
        /// <param name="Indicator">Reference to the indicator objects showing the move</param>
        public MoveInfo (Cell dest, Piece src, GameObject Indicator = null) 
        {
            cell = dest; 
            piece = src; 
            indicator = Indicator;
        }

        /// <summary> Reference to the move's target cell. </summary>
        public Cell cell;

        /// <summary> Reference to the piece being moved. </summary>
        public GameObject indicator;

        /// <summary> Reference to the indicator objects showing the move. </summary>
        public Piece piece;

        /// <summary>
        /// Whether or not the move will result in a capture. 
        /// </summary>
        /// <returns>bool</returns>
        public bool isAttack() 
        { 
            return cell.occupant != null && cell.occupant.Colour!=piece.Colour; 
        }
    }



    /// <summary>
    /// Node in the camera node graph. Contains a position,  and index of itself in the adjacency graph.
    /// </summary>
    public class CameraNode
    {
        /// <summary>
        /// Node in the camera node graph. Contains a position,  and index of itself in the adjacency graph.
        /// </summary>
        /// <param name="pos">Target position for the camera.</param>
        /// <param name="Idx">Index of the node in the graph.</param>
        public CameraNode(Vector3 pos, int Idx) 
        { 
            position = pos; 
            idx = Idx; 
        }

        /// <summary> Camera position. </summary>
        public Vector3 position;
        /// <summary> Index of the node in graph. </summary>
        public int idx;
    }

    /// <summary>
    /// Info for where to spawn a piece of a given type and colour.
    /// </summary>
    [System.Serializable]
    public class SpawnInfo
    {
        /// <summary>
        /// Info for where to spawn a piece of a given type and colour.
        /// </summary>
        /// <param name="Index">Index of the cell to spawn the piece at</param>
        /// <param name="Type">The type of piece to spawn</param>
        /// <param name="Colour">The piece's colour</param>
        public SpawnInfo(Vector3Int Index, PieceType Type, TeamColour Colour) 
        {
            type = Type; 
            colour = Colour;
            // determine piece position based on index
            pos = (2*Index) - new Vector3(7f,7f,7f);
        }

        /// <summary>
        /// Info for where to spawn a piece of a given type and colour.
        /// </summary>
        /// <param name="Index">The position to spawn the piece at</param>
        /// <param name="Type">The type of piece to spawn</param>
        /// <param name="Colour">The piece's colour</param>
        public SpawnInfo(Vector3 Position, PieceType Type, TeamColour Colour) 
        {
            type = Type; 
            colour = Colour; 
            pos = Position;
        }

        /// <summary> The position of the piece to spawn. </summary>
        public Vector3 pos;
        /// <summary> The type of piece to spawn. </summary>
        public PieceType type;
        /// <summary> The piece colour to spawn. </summary>
        public TeamColour colour;
    }
}

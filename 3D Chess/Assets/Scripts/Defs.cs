using UnityEngine;

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
}

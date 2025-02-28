using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.Rendering.Universal;
using UnityEngine;
using Defs;
using System.ComponentModel;

/// <summary>
/// Main storge for 3D array of cell objects.
/// </summary>
public class Board : MonoBehaviour
{
    /* ==========  MEMBERS  ========== */

    /// <summary> Prefab of the cell to be instantiated into the array. </summary>
    [SerializeField]
    private GameObject cell_prefab;
    /// <summary> xyz dimensions of the 3D array. </summary>
    [HideInInspector]
    public Vector3Int grid_dimensions = new Vector3Int(8, 8, 8);


    /// <summary> 3D array of cell objects. </summary>
    private Cell[,,] grid;
    /// <summary> Flag for when piece moves should be regenerated. </summary>
    private int regen_moves = 1;

    private List<SpawnInfo> initial_pieces;

    [SerializeField]
    private GameObject  blackPawnPrefab,
                        blackKnightPrefab,
                        blackBishopPrefab,
                        blackRookPrefab,
                        blackQueenPrefab,
                        blackKingPrefab,
                        whitePawnPrefab,
                        whiteKnightPrefab,
                        whiteBishopPrefab,
                        whiteRookPrefab,
                        whiteQueenPrefab,
                        whiteKingPrefab;
    
    [SerializeField]
    private GameObject whitePieceParent,
                       blackPieceParent;

    [SerializeField]
    private CellSelector selector;
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private KeyCode reset, quit;


    /* ==========  MAIN FUNCTIONS  ========== */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // intialise array
        grid = new Cell[grid_dimensions.x, grid_dimensions.y, grid_dimensions.z];

        // iterate over every (x,y,z) coordinate triple. Instantiate a new cell for each coordinate. 
        for (int x = 0; x < grid_dimensions.x; x++)
        {
            // Cell positions will begin at -7 on each axis, range until 7, and have a step of 2.
            // this will make the board centred at (0,0,0)
            float xpos = (2f * x) - 7f;
            for (int y = 0; y < grid_dimensions.y; y++)
            {
                float ypos = (2f * y) - 7f;
                for (int z = 0; z < grid_dimensions.z; z++)
                {
                    // Instantiate cell prefab as a child of cell container, and access the Cell script
                    Cell cell = Instantiate(
                        cell_prefab,
                        new Vector3(xpos, ypos, (2f * z) - 7f),
                        Quaternion.identity,
                        transform.GetChild(0))
                            .GetComponent<Cell>();
                    // set the cell's indices
                    cell.index = new Vector3Int(x, y, z);

                    // commit hte finished cell to the array 
                    grid[cell.index.x, cell.index.y, cell.index.z] = cell;
                }
            }
        }

        // populate initial pieces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        initial_pieces = new List<SpawnInfo>();
        foreach (GameObject obj in pieces)
        {
            Piece p = obj.GetComponent<Piece>();
            SpawnInfo info = new SpawnInfo(p.transform.position, p.Type, p.Colour);
            initial_pieces.Add(info);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (regen_moves != 0)
        {
            if (regen_moves--==1) RegenerateMoves(true);
        }

        if (Input.GetKeyDown(reset)) ResetGame();
        if (Input.GetKeyDown(quit)) QuitGame();
    }

    /// <summary>
    /// Get the cell at the given indices,
    /// </summary>
    /// <param name="index">xyz Vector3Int of the desired xyz indices</param>
    /// <returns>A reference to the cell at indices xyz. <c>null</c> if the indices are out of bounds.</returns>
    public Cell GetCellAt(Vector3Int index)
    {
        return GetCellAt(index.x, index.y, index.z);
    }

    /// <summary>
    /// Get the cell at the given indices.
    /// </summary>
    /// <param name="index">x, y, and z indices.</param>
    /// <returns>A reference to the cell at indices xyz. <c>null</c> if the indices are out of bounds.</returns>
    public Cell GetCellAt(int x, int y, int z)
    {
        // return null if index is out of bounds
        if (x < 0 || y < 0 || z < 0 || x >= grid_dimensions.x || y >= grid_dimensions.y || z >= grid_dimensions.z) return null;
        return grid[x, y, z];
    }




    /* ==========  HELPER FUNCTIONS  ========== */

    /// <summary>
    /// Toggles the index rendering for all cells in the grid.
    /// </summary>
    [ContextMenu("Toggle Indices")]
    public void ToggleIndices()
    {
        foreach (Cell cell in grid) cell.ToggleIndexDisplay();
    }

    /// <summary>
    /// <para>Regenerates the moves each piece is able to make. Also regenerates the pieces that can attack each cell.</para>
    /// 
    /// <para>If <c>checkDiscovered</c> is false, this will not check for potential checks caused by each piece's moves.</para>
    /// </summary>
    /// <param name="checkDiscovered">Indicate if pieces should check if their moves cause check by setting this to <c>true</c>.
    ///                               This will also instantiate move indicators for each piece's available moves.</param>
    public void RegenerateMoves(bool checkDiscovered)
    {
        // reset each cell's attackers
        foreach (Cell cell in grid)
        {
            cell.attackers = new Dictionary<TeamColour, List<Piece>>();
            // i know it says initialisation can be simplified, but trust me the alternative is ugly and less simple.
            cell.attackers.Add(TeamColour.White, new List<Piece>());
            cell.attackers.Add(TeamColour.Black, new List<Piece>());
        }

        // get a list of pieces as Piece objects
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Piece");
        List<Piece> pieces = new List<Piece>();
        foreach (GameObject obj in objs) {
            Piece p = obj.GetComponent<Piece>();
            if (p.Cell != null) pieces.Add(p);
        }

        // regenerate the moves for each piece
        foreach (Piece p in pieces) p.RegenerateMoves();

        if (checkDiscovered) 
        {
            // prune any moves that would cause check for the pieces' colour
            foreach (Piece p in pieces) p.PruneDiscovered();
            // add indicators for all moves. This DOES need to in a seperate loop because it needs to be called
            // AFTER all pieces have pruned their check-causing moves. 
            foreach (Piece p in pieces) p.AddIndicators();
        }
    }

    /// <summary>
    /// Checks if the given piece colour's king is currently in check in the given board position. Does this by locating
    /// the king, and chekcing if the cell it inhabits has any attackers of the opposite colour.
    /// </summary>
    /// <param name="player">The piece colour to query.</param>
    /// <returns><c>true</c> if the specified player is in check.</returns>
    private bool checkForInCheck(TeamColour player)
    {
        // get array of all pieces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        TeamColour opposite = player == TeamColour.White ? TeamColour.Black : TeamColour.White;

        foreach (GameObject obj in pieces)
        {
            Piece piece = obj.GetComponent<Piece>();
            // player king detected
            if (piece.Colour == player && piece.Type == PieceType.King)
            {
                // return whether the king is in check
                return piece.Cell.attackers[opposite].Count > 0;
            }
        }
        // no king found
        return false;
    }

    /// <summary>
    /// <para>Determines if a potential move will cause check by temporarily making the move, seeing if the king is in check, then reverting the move.</para>
    /// 
    /// <para>Calls <c>RegenerateMoves</c> with <c>checkDiscovered = false</c> to prevent infinite recursion.</para>
    /// </summary>
    /// <param name="targetCell">The cell to move a piece to.</param>
    /// <param name="sourceCell">The cell of the piece being moved.</param>
    /// <param name="team">The team colour to query for check.</param>
    /// <returns><c>true</c> if the move would result in the given team being in check.</returns>
    public bool willCauseCheck(Cell targetCell, Cell sourceCell, TeamColour team)
    {
        // Temporarily move the piece to the target cell
        Piece originalPiece = sourceCell.occupant;
        Piece targetPiece = targetCell.occupant;

        // temporarily update the pieces and cells
        if (targetPiece!=null) targetPiece.Cell = null;
        if (originalPiece!=null) originalPiece.Cell = targetCell;

        // check moves again based on the new positions 
        RegenerateMoves(false);

        // see if the king is in check in the temporary position
        bool inCheck = checkForInCheck(team);

        // Revert the move
        if (originalPiece!=null) originalPiece.Cell = sourceCell;
        if (targetPiece!=null) targetPiece.Cell = targetCell;

        // re-regenerate the moves with the original board position
        RegenerateMoves(false);

        return inCheck;
    }

    [ContextMenu("Reset Game")]
    public void ResetGame()
    {
        // clear all remaining pieces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject p in pieces) Destroy(p);

        // empty attackers/occupant of all cells
        foreach (Cell cell in grid)
        {
            cell.occupant = null;
            cell.attackers = new Dictionary<TeamColour, List<Piece>>();
            // i know it says initialisation can be simplified, but trust me the alternative is ugly and less simple.
            cell.attackers.Add(TeamColour.White, new List<Piece>());
            cell.attackers.Add(TeamColour.Black, new List<Piece>());
        }

        // spawn new pieces
        foreach (SpawnInfo piece_spawn in initial_pieces)
        {
            GameObject prefab = get_prefab(piece_spawn);
            GameObject parent = piece_spawn.colour==TeamColour.White? whitePieceParent : blackPieceParent;
            Instantiate(prefab, piece_spawn.pos, Quaternion.identity, parent.transform);
        }

        selector.gameObject.SetActive(true);
        selector.Cell = GetCellAt(0,0,0);
        if (selector.current_player!=TeamColour.White) selector.ChangeTurn();
        gameOverUI.SetActive(false);
        regen_moves = 2;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private GameObject get_prefab(SpawnInfo info)
    {
        switch (info.type)
        {
            case PieceType.Pawn: 
                return info.colour==TeamColour.White? whitePawnPrefab : blackPawnPrefab;
            case PieceType.Rook: 
                return info.colour==TeamColour.White? whiteRookPrefab : blackRookPrefab;
            case PieceType.Knight: 
                return info.colour==TeamColour.White? whiteKnightPrefab : blackKnightPrefab;
            case PieceType.Bishop: 
                return info.colour==TeamColour.White? whiteBishopPrefab : blackBishopPrefab;
            case PieceType.Queen: 
                return info.colour==TeamColour.White? whiteQueenPrefab : blackQueenPrefab;
            case PieceType.King: 
                return info.colour==TeamColour.White? whiteKingPrefab : blackKingPrefab;
        }
        return null;
    }
}
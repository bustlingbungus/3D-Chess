using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using Defs;

/// <summary>
/// 
/// <para>
/// The base class for all pieces on the board. On intstantiation, a piece will correct its position to the centre of the 
/// cell it inhabits.
/// </para>
/// 
/// <para><b>
/// Setting Colour and Type
/// </b></para>
/// 
/// <para>
/// Piece has two variable traits, <c>colour</c> and <c>type</c>. <c>Colour</c> is in reference to the 
/// player that controls the piece, e.g., white/black. <c>Type</c> is in reference to what the piece is, e.g., pawn, rook, etc.
/// </para>
/// 
/// <para><list type="bullet">
/// 
/// <item>
/// <description> <c>Colour</c> should be specified manually through the serialized field in the Unity inspector. </description>
/// </item>
/// 
/// <item>
/// <description> 
/// <c>Type</c> should be specified in the subclass' <c>Start</c> function, using <c>piece_init</c>.
/// 
/// <example><para><b>
/// Example:
/// </b></para>
/// 
/// <code>
/// public class Pawn : Piece
/// {
///     // Start is called once before the first execution of Update after the MonoBehaviour is created
///     void Start()
///     {
///         piece_init(PieceType.Pawn);     &lt;----- SET PIECE TYPE HERE
///     }
///     ...
///
/// </code></example>
/// </description></item>
/// </list></para>
/// 
/// <para><b>
/// Notes:
/// </b></para>
/// 
/// <list type="bullet">
/// 
/// <item>
/// <description>
/// A subclass' <c>Start</c> function should contain a call to the <c>piece_init</c> function. This function initialises
/// the piece's position and assigns provate members.
/// </description>
/// </item>
/// 
/// <item>
/// <description>
/// A subclass' <c>Update</c> function should contain a call to the <c>piece_update</c> function. This function handles the
/// piece's movement between cells.
/// </description>
/// </item>
/// 
/// </list>
/// 
/// </summary>
public abstract class Piece : MonoBehaviour
{
    /* ==========  SERIALISED FIELDS  ========== */

    /// <summary> The cell highlight to show on cells the piece can move to without capturing. </summary>
    [SerializeField]    
    private GameObject validMoveHighlightPrefab;
    /// <summary> The cell highlight to show on cells the piece can move to via capturing. </summary>
    [SerializeField]
    private GameObject attackMoveHighlightPrefab;

    /// <summary> The amount of time (in seconds) the piece will take to travel between two cells. </summary>
    [SerializeField]
    private float travelTime = 0.75f;

    /// <summary> Which player the piece belongs to. </summary>
    [SerializeField]
    private TeamColour colour = TeamColour.White;





    /* ==========  PROPERTIES  ========== */
    
    /// <summary> What kind of piece, e.g, pawn, queen, etc. </summary>
    public PieceType Type {
        get => _type;
    }
    /// <summary> Which player the piece belongs to. </summary>
    public TeamColour Colour {
        get => colour;
    }

    /// <summary>
    /// <para><b>get:</b> Returns the reference to the cell the piece is occupying. </para>
    /// <para><b>set:</b> Removes self from the current cell, and makes the destination cell (passed value) contain
    /// self as the occupant. If the destination cell contains a piece, destroys said piece.</para>
    /// </summary>
    public Cell Cell
    {
        get => _cell;
        set {
            if (value != null) {
                // remove self from current cell, set init pos for interpolation
                if (_cell!=null) {
                    _cell.occupant = null;
                    init_pos = _cell.transform.position;
                }
                // assign _cell
                _cell = value;
                // if the cell is occupied, destroy the occupant. Make self the new occupant
                if (_cell.occupant!=null) Destroy(_cell.occupant.gameObject);
                _cell.occupant = this;
                // reset timer for interpolation
                movement_timer = 0f;
            }
        }
    }





    /* ==========  MEMBERS  ========== */

    /// <summary> Reference to the board containing all cell references. </summary>
    [HideInInspector]
    public Board _board;
    /// <summary> Reference to the cell the piece is occupying. </summary>
    private Cell _cell;
    /// <summary> What kind of piece, e.g, pawn, queen, etc. </summary>
    private PieceType _type = PieceType.Pawn;

    /// <summary> Timer used for interpolation between previous position and new position when moving cells. </summary>
    private float movement_timer = 0f;
    /// <summary> The position of the cell previously occupied by the piece, used for interpolation between cells. </summary>
    private Vector3 init_pos = Vector3.zero;





    /* ==========  CONTEXT MENU  ========== */
    
    
    /// <summary>
    /// <para>Highlights cells that the piece can move to by instantiating valid/attack highlight prefabs in said cells. Detects cells to 
    /// highlight by calling the <c>find_valid_moves</c>, requiring an override of said function for this one to work.</para>
    /// 
    /// <para>The instantiated highlights are made as child objects of the piece.</para>
    /// 
    /// <see cref="find_valid_moves"/>
    /// </summary>
    [ContextMenu("Show Moves")]
    public void ShowMoves() {
        List<Move> moves = find_valid_moves();
        foreach (Move move in moves) {
            if (move.move_type==Move.MoveType.Regular) Instantiate(validMoveHighlightPrefab, move.cell.transform.position, Quaternion.identity, transform);
            else if (move.move_type == Move.MoveType.Attack) Instantiate(attackMoveHighlightPrefab, move.cell.transform.position, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// <para>Hides any cell highlights that show the spaces the piece can move to. Does this by searching the object's children, and
    /// destroying any child object with the tag <c>"Cell Highlight</c>.</para>
    /// </summary>
    [ContextMenu("Hide Moves")]
    public void HideMoves() {
        foreach (Transform child in transform) {
            if (child.tag == "Cell Highlight") Destroy(child.gameObject);
        }
    }





    /* ==========  HELPER FUNCTIONS  ========== */

    /// <summary>
    /// <para>Finds all moves the piece may currently make, by checking all cells the piece might be able to move to, and returning said
    /// cells if they are in bounds and not occupied or otherwise unable to be moved to.</para>
    /// 
    /// <para><c>Piece.find_valid_moves</c> isn't defined; this function must be defined in sub-classes.</para>
    /// </summary>
    /// <returns> A <c>List&lt;Move&gt;</c> of the mvoes the piece can make. </returns>
    public abstract List<Move> find_valid_moves();

    /// <summary>
    /// <para>Initialises the piece's rotation based on colour, initialises piece type with given parameter. Finds the cell the piece was placed within, 
    /// initialises the piece's reference to this cell, and corrects its position to be in the centre of the cell.</para>
    /// 
    /// <para>This function should be called once: within the sub-class' <c>Start</c> function.</para>
    /// </summary>
    /// <param name="type">The piece type, e.g. pawn, queen, etc.</param>
    public void piece_init(PieceType type)
    {
        _type = type;

        // rotate pieces 180 if they're black, 0 if they're white
        if (colour == TeamColour.Black) transform.eulerAngles = new Vector3(0f, 180f, 0f);
        else transform.eulerAngles = Vector3.zero;

        // intialise board reference
        _board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
        // find cell the piece was placed in
        look_for_cell();
        // set timer so pieces aren't moveing on instantiation
        movement_timer = travelTime;
    }

    /// <summary>
    /// <para>Ensures the piece contains a valid cell reference, and when the piece is moved, handles the interpolation for 
    /// moving the piece from cell A to cell B</para>
    /// 
    /// <para>This function should be called only within the sub-class' <c>Update</c> function.</para>
    /// </summary>
    public void piece_update()
    {
        // ensure cell reference is valid
        if (Cell==null) look_for_cell();

        // interpolate betwen last position and new cell position
        transform.position = pos_interp(init_pos, _cell.transform.position, movement_timer/travelTime);
        // update interpolation timer
        movement_timer = Mathf.Clamp(movement_timer+Time.deltaTime, 0f, travelTime);
    }

    /// <summary>
    /// <para>Finds the cell closest to the piece (which will be the cell the piece is within), and sets said cell to the piece's cell.</para>
    /// 
    /// <para>This is done in linear complexity with regard to the number of cells, by checking the distance to each cell once.</para>
    /// </summary>
    public void look_for_cell()
    {
        Cell best_cell = null;
        float best_dist = float.MaxValue;

        // iterate over cells and find the distance to each 
        foreach (Transform cell in _board.transform.GetChild(0)) {
            float disp = (cell.transform.position-transform.position).magnitude;
            // new closest cell found
            if (disp < best_dist) {
                best_dist = disp;
                best_cell = cell.GetComponent<Cell>();
            }
        }
        // update cell reference
        Cell = best_cell;
    }

    /// <summary>
    /// <para>Interpolation function from position A to position B for moving between cells. Uses interpolation function <i>0.5 * (sin(PI * (t - 0.5)) + 1)</i>.</para>
    /// 
    /// <para><c>t</c> should be the movement timer divided by travl time.</para>
    /// </summary>
    /// <param name="a">The starting position</param>
    /// <param name="b">The ending position</param>
    /// <param name="t">Interpolation value, 0-1</param>
    /// <returns>A <c>Vector3</c> representing the interpolated position.</returns>
    private Vector3 pos_interp(Vector3 a, Vector3 b, float t) {
        t = 0.5f*(Mathf.Sin(Mathf.PI*(t-0.5f))+1f);
        return a + t*(b - a);
    }
}

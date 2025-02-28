using UnityEngine;
using Defs;
using TMPro;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private Color whiteTurn, blackTurn;
    private Color col_a, col_b;
    [SerializeField]
    private float turnChangeTime = 1.0f;
    private float turn_timer;
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject movementUI;
    [SerializeField]
    private GameObject pieceMovingUI;
    [SerializeField]
    private GameObject selectPopUpUI;
    [SerializeField]
    private GameObject gameOverUI;

    private InputManager input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
        input = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    
        col_a = blackTurn;
        col_b = whiteTurn;
        turn_timer = turnChangeTime;

        cam.backgroundColor = whiteTurn;
    }

    // Update is called once per frame
    void Update()
    {
        if (cell_select_input())
        {
            LowerAlpha();
            move_select.enabled = true;
            movement_controls.enabled = false;
            move_select.GetMoves(cell.occupant);

            movementUI.SetActive(false);
            selectPopUpUI.SetActive(false);
            pieceMovingUI.SetActive(true);
        }
        else if (!move_select.enabled && cell!=null && cell.occupant!=null && cell.occupant.Colour==current_player) {
            selectPopUpUI.SetActive(true);
        } else selectPopUpUI.SetActive(false);

        cam.backgroundColor = colour_lerp(col_a, col_b, turn_timer / turnChangeTime);
        turn_timer = Mathf.Clamp(turn_timer+Time.deltaTime, 0f, turnChangeTime);
    }

    public Cell Cell
    {
        get => cell;
        set
        {
            if (value != null)
            {
                if (cell != null && cell.occupant != null) cell.occupant.HideMoves();
                cell = value;
                transform.position = cell.transform.position;
                if (cell.occupant != null && cell.occupant.Colour == current_player) cell.occupant.ShowMoves();
            }
        }
    }

    public TeamColour current_player = TeamColour.White;

    private Cell cell;

    [SerializeField]
    private PieceMover move_select;
    [SerializeField]
    private SelectorMover movement_controls;

    private Board _board;



    [SerializeField]
    private Material material;


    [ContextMenu("Lower Alpha")]
    public void LowerAlpha() { SetHighlightAlpha(0.05f); }
    [ContextMenu("Raise Alpha")]
    public void RaiseAlpha() { SetHighlightAlpha(0.588f); }

    public void SetHighlightAlpha(float newAlpha)
    {
        Color newCol = material.color;
        newCol.a = newAlpha;
        material.color = newCol;
        if (cell.occupant != null)
        {
            foreach (Transform child in cell.occupant.transform)
            {
                if (child.tag == "Cell Highlight")
                {
                    Material mat = child.GetComponent<MeshRenderer>().material;
                    newCol = mat.color;
                    newCol.a = newAlpha;
                    mat.color = newCol;
                }
            }
        }
    }


    [ContextMenu("Change Turn")]
    public void ChangeTurn()
    {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;
        current_player = current_player == TeamColour.White ? TeamColour.Black : TeamColour.White;
        _board.RegenerateMoves(true);
        LookForStalemate();

        // begin interpolating between background colours
        Color temp = col_a;
        col_a = col_b;
        col_b = temp;
        turn_timer = 0f;

        movementUI.SetActive(true);
        pieceMovingUI.SetActive(false);
    }

    public void ExitSelection()
    {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;

        movementUI.SetActive(true);
        pieceMovingUI.SetActive(false);
    }

    private bool cell_select_input()
    {
        return input.Select && !move_select.enabled &&
               cell != null && cell.occupant != null && cell.occupant.Colour == current_player;
    }

    private Color colour_lerp(Color a, Color b, float t) {
        return a + t * (b - a);
    }

    [ContextMenu("Look for checkmate")]
    void LookForStalemate()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        bool inCheck = false;
        TeamColour opposite = current_player == TeamColour.White ? TeamColour.Black : TeamColour.White;
        foreach (GameObject obj in pieces)
        {
            Piece piece = obj.GetComponent<Piece>();
            if (piece.Colour == current_player)
            {
                if (piece.available_moves.Count > 0)
                {
                    // Debug.Log("Not Checkmate");
                    return;
                }
                else if (piece.Type == PieceType.King)
                {
                    inCheck = piece.Cell.attackers[opposite].Count > 0;
                }
            }
        }
        
        EndGame(inCheck);
    }

    [ContextMenu("End game")]
    void ForceStalemate() { EndGame(false); }

    void EndGame(bool isCheckmate)
    {
        gameOverUI.SetActive(true);

        TMP_Text main_text = gameOverUI.transform.GetChild(1).GetComponent<TMP_Text>();
        TMP_Text sub_text = gameOverUI.transform.GetChild(2).GetComponent<TMP_Text>();

        if (isCheckmate) {
            main_text.SetText("Checkmate!");
            sub_text.SetText(current_player==TeamColour.White? "Black wins!":"White wins!");
        } else {
            main_text.SetText("Stalemate!");
            sub_text.SetText(" ");
        }

        gameObject.SetActive(false);
    }
}

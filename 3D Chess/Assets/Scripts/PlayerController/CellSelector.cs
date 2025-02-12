using UnityEngine;
using Defs;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode select = KeyCode.Return;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cell_select_input()) {
            LowerAlpha();
            move_select.enabled = true;
            movement_controls.enabled = false;
            move_select.GetMoves(cell.occupant);
        }
    }

    public Cell Cell
    {
        get => cell;
        set {
            if (value!=null) {
                if (cell!=null && cell.occupant!=null) cell.occupant.HideMoves();
                cell = value;
                transform.position = cell.transform.position;
                if (cell.occupant!=null && cell.occupant.Colour==current_player) cell.occupant.ShowMoves();
            }
        }
    }

    private TeamColour current_player = TeamColour.White;

    private Cell cell;

    [SerializeField]
    private PieceMover move_select;
    [SerializeField]
    private SelectorMover movement_controls;

    private Board _board;



    [SerializeField]
    private Material material;


    [ContextMenu("Lower Alpha")]
    public void LowerAlpha() { SetHighlightAlpha(0.15f); }
    [ContextMenu("Raise Alpha")]
    public void RaiseAlpha() { SetHighlightAlpha(0.588f); }

    public void SetHighlightAlpha(float newAlpha)
    {
        Color newCol = material.color;
        newCol.a = newAlpha;
        material.color = newCol;
        if (cell.occupant!=null) {
            foreach (Transform child in cell.occupant.transform) {
                if (child.tag == "Cell Highlight") {
                    Material mat = child.GetComponent<MeshRenderer>().material;
                    newCol = mat.color;
                    newCol.a = newAlpha;
                    mat.color = newCol;
                }
            }
        }
    }


    [ContextMenu("Change Turn")]
    public void ChangeTurn() {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;
        current_player = current_player==TeamColour.White?TeamColour.Black:TeamColour.White;
        _board.UpdateAttackers();
    }

    public void ExitSelection() {
        RaiseAlpha();
        move_select.enabled = false;
        movement_controls.enabled = true;
    }

    private bool cell_select_input() {
        return Input.GetKeyDown(select) && !move_select.enabled && 
               cell!=null && cell.occupant!=null && cell.occupant.Colour==current_player;
    }
}

using UnityEngine;
using Defs;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode select = KeyCode.Return,
                    cancel = KeyCode.Escape;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(select)) {
            if (move_select.enabled) {
                RaiseAlpha();
                move_select.SelectCurrent();
                move_select.enabled = false;
                movement_controls.enabled = true;
                ChangeTurn();
            } else if (cell!=null && cell.occupant!=null && cell.occupant.Colour==current_player) {
                LowerAlpha();
                move_select.enabled = true;
                move_select.GetMoves(cell.occupant);
                movement_controls.enabled = false;
            }
        } else if (Input.GetKeyDown(cancel) && move_select.enabled) {
            RaiseAlpha();
            move_select.enabled = false;
            movement_controls.enabled = true;
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
    private MoveSelector move_select;
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
        current_player = current_player==TeamColour.White?TeamColour.Black:TeamColour.White;
        _board.UpdateAttackers();
    }
}

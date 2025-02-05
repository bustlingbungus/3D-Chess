using UnityEngine;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode select = KeyCode.Return,
                    cancel = KeyCode.Escape;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            } else if (cell!=null && cell.occupant!=null) {
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
                if (cell.occupant!=null) cell.occupant.ShowMoves();
            }
        }
    }

    private Cell cell;

    [SerializeField]
    private MoveSelector move_select;
    [SerializeField]
    private SelectorMover movement_controls;



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
}

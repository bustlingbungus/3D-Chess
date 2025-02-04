using UnityEngine;

public class CellSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode next_move = KeyCode.RightArrow,
                    prev_move = KeyCode.LeftArrow,
                    select = KeyCode.Return;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

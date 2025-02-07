using System.Xml.Serialization;
using UnityEngine;

public class SelectorMover : MonoBehaviour
{
    public CellSelector selector;
    private Board board;

    [SerializeField]
    private KeyCode positiveX = KeyCode.W,
                    negativeX = KeyCode.S,
                    positiveY = KeyCode.UpArrow,
                    negativeY = KeyCode.DownArrow,
                    positiveZ = KeyCode.A,
                    negativeZ = KeyCode.D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Main Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selector.Cell==null) selector.Cell = board.GetCellAt(0, 0, 0);
        else {
            Vector3 base_z = Camera.main.transform.forward, base_y=Vector3.zero, base_x=Vector3.zero;
            Vector3.OrthoNormalize(ref base_z, ref base_y, ref base_x);


            if (base_z.z==1f) { base_x*=-1f; base_y*=-1f; }
            else if (base_z.x==-1f) { base_x*=-1f; base_y*=-1f; }
            else if (base_z.y != 0f) {
                Vector3 temp = base_x;
                base_x = base_y;
                base_y = temp;
                if (base_z.y==-1) base_x *= -1;
                else base_y *= -1;
            }

            Vector3 index = selector.Cell.index;

            if (Input.GetKeyDown(negativeZ)) base_z *= -1f;
            else if (!Input.GetKeyDown(positiveZ)) base_z = Vector3.zero;
            if (Input.GetKeyDown(negativeX)) base_x *= -1f;
            else if (!Input.GetKeyDown(positiveX)) base_x = Vector3.zero;
            if (Input.GetKeyDown(negativeY)) base_y *= -1f;
            else if (!Input.GetKeyDown(positiveY)) base_y = Vector3.zero;

            index += base_z + base_y + base_x;
            Vector3Int idx = new Vector3Int((int)index.x,(int)index.y,(int)index.z);

            if (idx!=selector.Cell.index) selector.Cell = board.GetCellAt(idx);
        }
    }
}
